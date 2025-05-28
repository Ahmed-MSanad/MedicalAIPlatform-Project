using AiDrivenMedicalPlatformAPIs.Extensions;
using AiDrivenMedicalPlatformAPIs.Controllers;
using MedicalProj.Data.Contexts.Contracts.Interfaces;
using MedicalProj.Data.Contexts.Contracts.Classes;

namespace AiDrivenMedicalPlatformAPIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();

            builder.Services.AddSwaggerExplorer()
                            .InjectDbContext(builder.Configuration) // Injecting an instance of the DB context Class => ** InjectDbContext is an Extension Method ** Using the model classes that we have inside the identity store Layer -> let's create the actual physical DB inside the SQL server.
                            .AddAppConfig(builder.Configuration) // Adding AppSettings => ** AddAppConfig is an Extension Method **
                            .AddIdentityHandlersAndStores() // adding ASP.NET Identity Core Services => ** AddIdentityHandlersAndStores is an Extension Method ** 
                            .ConfigureIdentityOptions() // Adjust Validators => ** ConfigureIdentityOptions is an Extension Method ** 
                            .AddIdentityAuth(builder.Configuration); // Adding Login Authentication Needed => ** AddIdentityAuth is an Extension Method **

            builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
            //builder.Services.AddSingleton<IEmailService, EmailService>();


            builder.Services.AddScoped<IDbInitializer, DbInitializer>();

            var app = builder.Build();

            await SeedDbAsync(app);

            app.UseHttpsRedirection();

            // Configure Swagger (the HTTP request pipeline) => ConfiguerSwaggerExplorer is a custom extension method => 
            app.ConfiguerSwaggerExplorer()
               .ConfigureCORS(builder.Configuration) // ConfigureCORS is a custom extension method => Config CORS
               .AddIdentityAuthMiddleWares(); // Adding Identity Authentication Middlewares => ** AddIdentityAuthMiddleWares is an Extension Method **


            app.MapControllers();

            //app.MapGroup("/api").MapIdentityApi<AppUser>();


            app.MapGroup("/api")
               .MapIdentityUserEndPoints() // Registration + Login
               .MapAuthorizationEndPoints()
               .MapScheduleEndpoints();
               


            app.Run();
        }

        static async Task SeedDbAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeAsync();
        }
    }
}