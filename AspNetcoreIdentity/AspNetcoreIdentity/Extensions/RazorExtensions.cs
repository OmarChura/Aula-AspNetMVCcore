using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
namespace AspNetcoreIdentity.Extensions
{
    public static class RazorExtensions
    {
        public static bool IfClaim(this RazorPage page, string claimName, string clamiValue)
        {
            return CustomAuthorization.ValidarClaimUsuarios(page.Context, claimName, clamiValue);
        }

        public static string IfClaimShow(this RazorPage page, string claimName, string clamiValue)
        {
            return CustomAuthorization.ValidarClaimUsuarios(page.Context, claimName, clamiValue) ? "" : "disabled";
        }

        public static IHtmlContent IfClaimShow(this IHtmlContent page, HttpContext context, string claimName, string clamiValue)
        {
            return CustomAuthorization.ValidarClaimUsuarios(context, claimName, clamiValue) ? page : null;
        }
    }
}
