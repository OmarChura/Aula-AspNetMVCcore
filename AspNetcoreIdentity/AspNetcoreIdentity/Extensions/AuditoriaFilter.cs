using KissLog;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AspNetcoreIdentity.Extensions
{
    public class AuditoriaFilter : IActionFilter
    {
        private readonly IKLogger _logger;
        public AuditoriaFilter(IKLogger logger)
        {
            _logger = logger;
        }   

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                var message = context.HttpContext.User.Identity.Name + "Acessou: " +
                    context.HttpContext.Request.GetDisplayUrl();

                _logger.Info(message);
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
           
        }
    }
}
