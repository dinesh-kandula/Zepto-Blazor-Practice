using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Net.Mime.MediaTypeNames;
using ModelsClassLibrary.Models.DTO;
using Zepto_API_Backup.Services;
using Zepto_API_Backup.Filter;
using ModelsClassLibrary.Models;
using Microsoft.EntityFrameworkCore;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zepto_API_Backup.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        private string SendEmail()
        {
            string senderEmail = "dinesh.kandula@techvedika.com";
            string senderPassword = "Bunny1997@"; //teyj vzvw cdcy cjaf
            string receiverEmail = "dineshkandula007@gmail.com"; // Receiver's email address
            string smtpHost = "smtp.outlook.com"; // SMTP server settings (e.g., for Gmail)
            int smtpPort = 587; // Gmail SMTP port

            var email = new MailMessage();
            email.From = new MailAddress(senderEmail);
            email.To.Add(new MailAddress(receiverEmail));
            email.Subject = "Test Email from C#";
            email.IsBodyHtml = true;
            email.Body = $"""
                        <h3>Greatings</h3>    
                        <br/>
                        <h2>This is a test email sent from a C# program</h2>
                        <p>Here is your OTP <span style="font-weight:bold;">1234</span></p>
                        <br/>
                        <h3>Thanks.</h3>
                    """;

            using (var smtp = new SmtpClient()
            {
                Port = smtpPort,
                Host = smtpHost,
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                UseDefaultCredentials = false
            })
            {
                try
                {
                    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    // Send the email
                    smtp.Send(email);
                    Console.WriteLine("Email sent successfully!");
                    return "Email send successfully";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to send email: {ex.Message}");
                    return $"Failed to send email: {ex.Message}";
                }
                finally
                {
                    // Dispose of the SmtpClient and MailMessage objects
                    smtp.Dispose();
                    email.Dispose();
                }
            }

        }

        [HttpPost("/Register")]
        [AuthAtribute(["Admin"])]
        public async Task<ActionResult> AddUser([FromForm] ZeptoUserDTO user)
        {
            try
            {
                Guid result = await _unitOfWork.UserRepository.ADOCreateAsync(user);
                return Ok($"User Details added successfully with user Id: {result}");
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                        "Error retrieving data from the database");
            }
        }

        [HttpPost("/Login")]
        public async Task<ActionResult> Login([FromForm] UserLoginDTO credentials)
        {
            if (credentials == null)
                return StatusCode(StatusCodes.Status400BadRequest, "Pass the valid Credentials to Login");

            ZeptoUser userDetails = await _unitOfWork.UserRepository.ADOGetUserAsync(credentials.UserName);

            if (userDetails == null)
                return StatusCode(StatusCodes.Status404NotFound, "Invalid Username! No user found");

            bool passwordMatch = ValidateUserPassword(userDetails, credentials.Password);

            if (!passwordMatch)
                return StatusCode(StatusCodes.Status401Unauthorized, "Password doesn't Match");

            var tokens = await GenerateTokensAsync(userDetails);
            return Ok(tokens);
        }

        [HttpPost("/RefreshToken")]
        public ActionResult RefreshToken([FromBody] AuthTokenDTO tokenDto)
        {
            if (tokenDto == null || string.IsNullOrEmpty(tokenDto.AccessToken) || string.IsNullOrEmpty(tokenDto.RefreshToken))
                return BadRequest("Invalid client request");

            var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
            if (principal == null)
                return BadRequest("Invalid access token or refresh token");

            var username = principal.Identity.Name;
            var user = _unitOfWork.UserRepository.ADOGetUserAsync(username).Result;

            if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return StatusCode(StatusCodes.Status401Unauthorized, "Invalid refresh token or refresh token has expired");

            var newAccessToken = GenerateAccessToken(principal.Claims);
            var newRefreshToken = tokenDto.RefreshToken;

            return Ok(new AuthTokenDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            });
        }

        private async Task<AuthTokenDTO> GenerateTokensAsync(ZeptoUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName), // Set the Name claim
                new Claim("username", user.UserName),
                new Claim("Role", user.UserType.ToString()),
                new Claim("Email", user.Email ?? string.Empty)
            };

            //string authKey = _configuration["Jwt:Key"];
            //string issuer = _configuration["Jwt:Issuer"];
            //string audience = _configuration["Jwt:Audience"];
            string authKey = "G3VF4C6KFV43JH6GKCDFGJH45V36JHGV3H4C6F3GJC63HG45GH6V345GHHJ4623FJL3HCVMO1P23PZ07W8";
            string issuer = "dinesh";
            string audience = "Public";
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: signinCredentials
            );

            string accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            var randomNumber = new byte[32];
            string refreshToken;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
            }

            // Set the refresh token and expiry time
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(20); // Set the expiry time as needed
            
            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.CompleteAsync();

            AuthTokenDTO tokens = new AuthTokenDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return tokens;
        }

        private string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            //string authKey = _configuration["Jwt:Key"];
            //string issuer = _configuration["Jwt:Issuer"];
            //string audience = _configuration["Jwt:Audience"];
            string authKey = "G3VF4C6KFV43JH6GKCDFGJH45V36JHGV3H4C6F3GJC63HG45GH6V345GHHJ4623FJL3HCVMO1P23PZ07W8";
            string issuer = "dinesh";
            string audience = "Public";
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("G3VF4C6KFV43JH6GKCDFGJH45V36JHGV3H4C6F3GJC63HG45GH6V345GHHJ4623FJL3HCVMO1P23PZ07W8")),
                ValidateLifetime = false // Here we are saying that we don't care about the token's expiration date
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;
        }

        private bool ValidateUserPassword(ZeptoUser user, string userReqestedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(userReqestedPassword, user.Password);
        }
    }
}
