using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AiDrivenMedicalPlatformAPIs.Models;
using AiDrivenMedicalPlatformAPIs.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using static AiDrivenMedicalPlatformAPIs.Program;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    public static class IdentityUserEndPoints
    {
        public static IEndpointRouteBuilder MapIdentityUserEndPoints(this IEndpointRouteBuilder app)
        {

            app.MapPost("/signup", CreateUser).AllowAnonymous();
            

            app.MapPost("/signin", SignInUser);

            return app;
        }

        private static async Task<IResult> CreateUser(UserManager<AppUser> userManager, [FromBody] UserRegistrationModel userRegistrationModel)
        {
            AppUser user = new AppUser
            {
                UserName = userRegistrationModel.Email,
                Email = userRegistrationModel.Email,
                FullName = userRegistrationModel.FullName,
                PhoneNumber = userRegistrationModel.PhoneNumber,
                DateOfBirth = userRegistrationModel.DateOfBirth,
                Gender = userRegistrationModel.Gender,
                Address = userRegistrationModel.Address,
                Occupation = userRegistrationModel.Occupation,
                EmergencyContactName = userRegistrationModel.EmergencyContactName,
                EmergencyContactNumber = userRegistrationModel.EmergencyContactNumber,
                FamilyMedicalHistory = userRegistrationModel.FamilyMedicalHistory,
                PastMedicalHistory = userRegistrationModel.PastMedicalHistory
            };
            var result = await userManager.CreateAsync(user, userRegistrationModel.Password);

            await userManager.AddToRoleAsync(user, userRegistrationModel.Role); // Adding the role to the user

            if (result.Succeeded)
            {
                return Results.Ok(result);
            }
            else
            {
                return Results.BadRequest(result);
            }
        }

        [AllowAnonymous]
        private static async Task<IResult> SignInUser(UserManager<AppUser> userManager, 
                                                      [FromBody] LoginModel loginModel, 
                                                      IOptions<AppSettings> appSettings) // Injecting the AppSettings
        {
            var user = await userManager.FindByEmailAsync(loginModel.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var roles = await userManager.GetRolesAsync(user);
                // Generate Token:
                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.JWTSecret));
                // Describing the claims that we want to include in the JWT payload:
                ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserID", user.Id.ToString()),
                    new Claim("Gender", user.Gender.ToString()),
                    new Claim("Age", (DateTime.Now.Year - user.DateOfBirth.Year).ToString()),
                    new Claim(ClaimTypes.Role, roles.First()),
                });
                if(user.HospitalId != null)
                {
                    claims.AddClaim(new Claim("HospitalId", user.HospitalId.ToString()!));
                }
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddDays(5), // Token expiration time
                                                          // Signing the token with the key:
                    SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature),
                };
                // Creating the token:
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                return Results.Ok(new { token });
            }
            else
            {
                return Results.BadRequest(new { message = "Invalid email or password!" });
            }
        }
    
    }

    public class UserRegistrationModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string FamilyMedicalHistory { get; set; }
        public string PastMedicalHistory { get; set; }
        public string Role { get; set; }
    }
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
