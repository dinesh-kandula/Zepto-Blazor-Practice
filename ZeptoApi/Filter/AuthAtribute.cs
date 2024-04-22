using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace ZeptoApi.Filter
{
    public class AuthAtribute : TypeFilterAttribute
    {
        public AuthAtribute(string actionName) : base(typeof(AuthorizeAction))
        {
            Arguments = [actionName];
        }
    }

    public class AuthorizeAction : IAuthorizationFilter
    {
        private readonly string _actionName;

        public AuthorizeAction(string actionName)
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
                    if (!string.IsNullOrEmpty(_actionName))
                    {
                        bool output = userType.Contains(_actionName);
                        if (!output)
                            context.Result = new JsonResult("Permission denined!");
                    }
                }
                else
                    context.Result = new JsonResult("Permission denined!");
            }
            else
                context.Result = new JsonResult("Permission denined!");
        }
    }
}
