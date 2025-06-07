using Services.Abstraction;
using Services;
using Shared;

namespace AiDrivenMedicalPlatformAPIs.Extensions
{
    static public class CoreServicesExtension
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IServiceManager, ServiceManager>();

            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));

            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
