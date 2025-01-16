using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace Zepto_API_Backup.Filter
{
    public class AuthAtribute : TypeFilterAttribute
    {
        public AuthAtribute(string[] actionName) : base(typeof(AuthorizeAction))
        {
            Arguments = [actionName];
        }
    }

    public class AuthorizeAction : IAuthorizationFilter
    {
        private readonly string[] _actionName;

        public AuthorizeAction(string[] actionName)
        {
            _actionName = actionName;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var handler = new JwtSecurityTokenHandler();
            string authHeader = context.HttpContext.Request.Headers.Authorization!;
            if (!String.IsNullOrEmpty(authHeader))
            {
                authHeader = authHeader.Replace("Bearer ", "");
                var jsonToken = handler.ReadToken(authHeader);
                if (handler.ReadToken(authHeader) is JwtSecurityToken tokenS)
                {
                    var userType = tokenS.Claims.First(claim => claim.Type == "Role").Value;
                    if (_actionName.Length > 0)
                    {
                        bool output = _actionName.Contains(userType);
                        if (!output)
                            context.Result = ErrorJSONMessage401();
                    }
                }
                else
                    context.Result = ErrorJSONMessage401();
            }
            else
                context.Result = ErrorJSONMessage401();
        }
        public JsonResult ErrorJSONMessage401()
        {
            return new JsonResult($"Permission denined! Only {string.Join(",", _actionName)} can access") 
            { StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}
