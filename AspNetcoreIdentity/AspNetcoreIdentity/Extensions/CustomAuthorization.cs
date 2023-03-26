using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace AspNetcoreIdentity.Extensions
{
    public class CustomAuthorization
    {

        public static Boolean ValidarClaimUsuarios(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated &&
                context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }

    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {

        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }

    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        readonly Claim _claim;

        public RequisitoClaimFilter(Claim claim)
        {
            _claim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "Identity", page = "/Account/Login", ReturnUrl = context.HttpContext.Request.Path.ToString() }));
            }

            if (!CustomAuthorization.ValidarClaimUsuarios(context.HttpContext, _claim.Type, _claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
