using MedicalProj.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AiDrivenMedicalPlatformAPIs.Extensions
{
    public static class EFCoreExtensions
    {
        public static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<MedicalDbContext>(option =>
                option.UseSqlServer(config.GetConnectionString("DevDB")));
            return services;
        }
    }
}
