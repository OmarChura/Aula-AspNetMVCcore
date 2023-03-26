
using AspNetcoreIdentity.Areas.Identity.Data;
using AspNetcoreIdentity.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetcoreIdentity.Configurations
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddAuthorizationConfig(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PodeExcluir", policy => policy.RequireClaim("PodeExcluir"));

                options.AddPolicy("PodeLer", policy => policy.Requirements.Add(new PermissaoNecesaria("PodeLer")));
                options.AddPolicy("PodeEscrever", policy => policy.Requirements.Add(new PermissaoNecesaria("PodeEscrever")));
            });

            return services;
        }
        public static IServiceCollection AddIdentityConfig(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("AspNetcoreIdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'AspNetcoreIdentityContextConnection' not found.");

            // Adicionando suporte ao contexto do Identity via EF
            services.AddDbContext<AspNetcoreIdentityContext>(options =>
                options.UseSqlServer(connectionString));

            // Adicionando suporte ao contexto do Identity via EF
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AspNetcoreIdentityContext>();

            return services;
        }
    }
}
