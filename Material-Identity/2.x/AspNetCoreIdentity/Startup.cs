using AspNetCoreIdentity.Config;
using AspNetCoreIdentity.Extensions;
using KissLog;
using KissLog.Apis.v1.Listeners;
using KissLog.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreIdentity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IHostingEnvironment hostEnvironment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(hostEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsProduction())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityConfig(Configuration);
            services.AddAuthorizationConfig();
            services.ResolveDependencies();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(AuditoriaFilter));

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/erro/500");
                app.UseStatusCodePagesWithRedirects("/erro/{0}");
                app.UseHsts();
            }

            app.UseKissLogMiddleware();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default","{controller=Home}/{action=Index}/{id?}");
            });

            LogConfig.RegisterKissLogListeners(Configuration);
        }
    }
}
