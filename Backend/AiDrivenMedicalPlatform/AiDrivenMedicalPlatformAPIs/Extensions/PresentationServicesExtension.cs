using Microsoft.OpenApi.Models;

namespace AiDrivenMedicalPlatformAPIs.Extensions
{
    static public class PresentationServicesExtension
    {
        public static IServiceCollection AddPresentationServices(this IServiceCollection services)
        {
            services.AddControllers();

            //services.AddControllers().AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            //});

            services.ConfigureSwagger();

            return services;
        }

        public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer()
                    .AddSwaggerGen(options =>
                    {
                        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                        {
                            Name = "Authorization",
                            Type = SecuritySchemeType.Http,
                            Scheme = "Bearer",
                            BearerFormat = "JWT",
                            In = ParameterLocation.Header,
                            Description = "Fill in the JWT token",
                        });
                        options.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    }
                                },
                                new List<string>()
                            }
                        });
                    });
            return services;
        }
    }
}
