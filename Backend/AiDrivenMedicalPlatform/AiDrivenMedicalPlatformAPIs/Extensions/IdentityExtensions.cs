using System.Text;
using AiDrivenMedicalPlatformAPIs.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AiDrivenMedicalPlatformAPIs.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityHandlersAndStores(this IServiceCollection services)
        {
            services.AddIdentityApiEndpoints<AppUser>() // For user the default entity is identityUser But we've customized it into AppUser
                    .AddRoles<IdentityRole>() // here we don't have any customization for the roles so we can use the default IdentityRole
                    .AddEntityFrameworkStores<AppDbContext>();
            return services;
        }

        public static IServiceCollection ConfigureIdentityOptions(this IServiceCollection services)
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

        public static IServiceCollection AddIdentityAuth(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(y =>
            {
                y.SaveToken = false; // Don't save the token inside the server once it's generated.
                // Validate the token
                y.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true, // validate the token signature
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        config["AppSettings:JWTSecret"]!)),
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
            });
            return services;
        }

        public static WebApplication AddIdentityAuthMiddleWares(this WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
