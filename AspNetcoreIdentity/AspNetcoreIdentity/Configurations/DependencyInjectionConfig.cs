
using AspNetcoreIdentity.Areas.Identity.Data;
using AspNetcoreIdentity.Extensions;
using KissLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetcoreIdentity.Configurations
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, PermissaoNecessariaHandler>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(context => Logger.Factory.Get());

            services.AddScoped<AuditoriaFilter>();

            return services;
        }

        
    }
}
