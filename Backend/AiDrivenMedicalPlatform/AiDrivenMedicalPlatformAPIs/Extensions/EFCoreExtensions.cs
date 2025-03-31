using AiDrivenMedicalPlatformAPIs.Models;
using Microsoft.EntityFrameworkCore;

namespace AiDrivenMedicalPlatformAPIs.Extensions
{
    public static class EFCoreExtensions
    {
        public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<AppDbContext>(option =>
                option.UseSqlServer(config.GetConnectionString("DevDB")));
            return services;
        }
    }
}
