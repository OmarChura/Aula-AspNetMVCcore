

using KissLog;
using KissLog.AspNetCore;
using KissLog.CloudListeners.Auth;
using KissLog.CloudListeners.RequestLogsListener;
using KissLog.Formatters;
using Microsoft.Extensions.Configuration;

namespace AspNetcoreIdentity.Configurations
{
    public static class LogConfig
    {
        public static void RegisterKissLogListeners(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();

            // Optional. Register IKLogger if you use KissLog.IKLogger instead of Microsoft.Extensions.Logging.ILogger<>
            services.AddScoped<IKLogger>((provider) => Logger.Factory.Get());

            services.AddLogging(logging =>
            {
                logging.AddKissLog(options =>
                {
                    options.Formatter = (FormatterArgs args) =>
                    {
                        if (args.Exception == null)
                            return args.DefaultValue;

                        string exceptionStr = new ExceptionFormatter().Format(args.Exception, args.Logger);

                        return string.Join(Environment.NewLine, new[] { args.DefaultValue, exceptionStr });
                    };
                });
            });
        }
        public static void RegisterKissLogListeners(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseKissLogMiddleware(options => ConfigureKissLog(options, configuration));
        }
        public static void ConfigureKissLog(IOptionsBuilder options, IConfiguration configuration)
        {
            KissLogConfiguration.Listeners.Add(new RequestLogsApiListener(new Application(
                        configuration["KissLog.OrganizationId"],    //  "552e02a2-8432-4fdb-b919-ea5c021b2c4f"
                        configuration["KissLog.ApplicationId"])     //  "04966dbc-6933-4270-8905-16cc9dab4a37"
                    )
            {
                ApiUrl = configuration["KissLog.ApiUrl"]    //  "https://api.kisslog.net"
            });
        }
    }
}
