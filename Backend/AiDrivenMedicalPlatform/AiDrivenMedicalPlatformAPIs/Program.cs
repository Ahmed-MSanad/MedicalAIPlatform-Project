
using AiDrivenMedicalPlatformAPIs.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Http.HttpResults;
using AiDrivenMedicalPlatformAPIs.Types;
using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using AiDrivenMedicalPlatformAPIs.Extensions;
using AiDrivenMedicalPlatformAPIs.Controllers;

namespace AiDrivenMedicalPlatformAPIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container =>

            builder.Services.AddControllers();

            builder.Services.AddSwaggerExplorer()
                            .InjectDbContext(builder.Configuration) // Injecting an instance of the DB context Class => ** InjectDbContext is an Extension Method ** Using the model classes that we have inside the identity store Layer -> let's create the actual physical DB inside the SQL server.
                            .AddAppConfig(builder.Configuration) // Adding AppSettings => ** AddAppConfig is an Extension Method **
                            .AddIdentityHandlersAndStores() // adding ASP.NET Identity Core Services => ** AddIdentityHandlersAndStores is an Extension Method ** 
                            .ConfigureIdentityOptions() // Adjust Validators => ** ConfigureIdentityOptions is an Extension Method ** 
                            .AddIdentityAuth(builder.Configuration); // Adding Login Authentication Needed => ** AddIdentityAuth is an Extension Method **


            var app = builder.Build();

            // Configure Swagger (the HTTP request pipeline) => ConfiguerSwaggerExplorer is a custom extension method => 
            app.ConfiguerSwaggerExplorer()
               .ConfigureCORS(builder.Configuration) // ConfigureCORS is a custom extension method => Config CORS
               .AddIdentityAuthMiddleWares(); // Adding Identity Authentication Middlewares => ** AddIdentityAuthMiddleWares is an Extension Method **

            app.UseHttpsRedirection();

            app.MapControllers();

            app.MapGroup("/api").MapIdentityApi<AppUser>();

            
            app.MapGroup("/api")
               .MapIdentityUserEndPoints() // Registration + Login
               .MapProfileEndpoints()
               .MapAuthorizationEndPoints();


            app.Run();
        }
    }
}
