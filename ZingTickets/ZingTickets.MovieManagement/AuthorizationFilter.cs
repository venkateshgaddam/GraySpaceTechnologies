using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.JsonWebTokens;

namespace ZingTickets.MovieManagement
{
    public class AuthorizationFilter : ActionFilterAttribute
    {
        private string SecretKey = "18081j6393m3bb97p9i2se46l9pakd4j4tenmlqlg1av4m4ajo0r";

        public AuthorizationFilter() { }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            var request = filterContext.HttpContext.Request;
            string baseUrl = $"{request.Scheme}://{request.Host}{request.QueryString.Value?.TrimEnd('/')}";
            string token = string.Empty;

            if (request.HasFormContentType)
            {
                token = request.Form["jwt"].FirstOrDefault();
            }

            //var jsonPayload = JsonWebToken.DecodeToObject(token, SecretKey) as IDictionary<string, object>;

            base.OnActionExecuting(filterContext);
        }
    }
}
