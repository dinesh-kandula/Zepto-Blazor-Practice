using Microsoft.AspNetCore.Mvc;
using ModelsClassLibrary.Models;
using ModelsClassLibrary.Models.DTO;
using System.Net.Mail;
using System.Net;
using MimeKit;
using ModelsClassLibrary.Services;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Cryptography;
using Azure.Core;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ZeptoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet()]
        public string SendEmail()
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

            var tokens = GenerateTokens(userDetails);

            return Ok(tokens);
        }

        private AuthTokenDTO GenerateTokens(ZeptoUser user)
        {
            var claims = new List<Claim>
                            {
                                new Claim("username", user.UserName),
								// Add roles as multiple claims
								new Claim("Role", user.UserType.ToString()),
                                new Claim("Email",(user.Email!=null?user.Email:string.Empty))

                            };
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


            string accessToken = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            var randomNumber = new byte[32];
            string refreshToken;
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                refreshToken = Convert.ToBase64String(randomNumber);
            }

            AuthTokenDTO tokens = new AuthTokenDTO()
            {
                JWTAccessToken = accessToken,
                RefreshToken = refreshToken
            };
            return tokens;
        }

        private bool ValidateUserPassword(ZeptoUser user, string userReqestedPassword)
        {

            return BCrypt.Net.BCrypt.Verify(userReqestedPassword, user.Password);
        }

        // GET api/<UserController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<UserController>
        //[HttpPost]
        //public void Post([FromBody] User userData)
        //{


        //}

        //// PUT api/<UserController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<UserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
