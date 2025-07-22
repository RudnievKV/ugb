using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using WebApi.Services.Abstractions;

namespace WebApi.Utils
{
    public class JwtAuthenticateAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("x-auth-token", out var token))
            {
                context.Result = new UnauthorizedObjectResult(new { Message = "Missing x-auth-token header" });
                return;
            }

            var validator = context.HttpContext.RequestServices.GetService<ILoginService>();
            if (validator == null || !validator.ValidateToken(token!))
            {
                context.Result = new UnauthorizedObjectResult(new { Message = "Invalid token" });
            }
        }
    }
}
