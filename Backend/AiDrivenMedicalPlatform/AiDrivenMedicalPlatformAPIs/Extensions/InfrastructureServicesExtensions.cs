using MedicalProj.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Data;
using Services.Abstraction;
using Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MedicalProj.Data.Models;

namespace AiDrivenMedicalPlatformAPIs.Extensions
{
    public static class InfrastructureServicesExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IDbInitializer, DbInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IServiceManager, ServiceManager>();

            services.AddDbContext<MedicalDbContext>(option => option.UseSqlServer(configuration.GetConnectionString("DevDB")));

            services.AddIdentityApiEndpoints<AppUser>() // For user the default entity is identityUser But we've customized it into AppUser
                            .AddRoles<IdentityRole>() // here we don't have any customization for the roles so we can use the default IdentityRole
                            .AddEntityFrameworkStores<MedicalDbContext>();

            services.ConfigureIdentity();

            services.ConfigureJWT(configuration);

            return services;
        }

        private static IServiceCollection ConfigureIdentity(this IServiceCollection services)
        {
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 6;
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }

        private static IServiceCollection ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(y =>
            {
                y.SaveToken = false; // Don't save the token inside the server once it's generated.
                // Validate the token
                y.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // validate the token signature
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        configuration["AppSettings:JWTSecret"]!)),
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero, // To avoid consider Time Standard.
                };
            });

            services.AddAuthorization(options =>
            {
                // The user has to be authenticated to the system to access any Web API method from this project except those which take the [AllowAnonymous]:
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

                // The user has to be authorized by having an id in the hospital to use any method has this policy:
                options.AddPolicy("HasHospitalId", policy => policy.RequireClaim("HospitalId"));


                options.AddPolicy("FemaleOnly", policy => policy.RequireClaim("Gender", "Female"));


                options.AddPolicy("Above18", policy => policy.RequireAssertion(context =>
                                                    Int32.Parse(context.User.Claims.First(x => x.Type == "Age").Value) > 18));

                options.AddPolicy("PatientsAndAdminsOnly", policy => policy.RequireRole("Patient", "Admin"));
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("PatientOnly", policy => policy.RequireRole("Patient"));
            });

            return services;
        }
    }
}
