using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction;
using Shared;
using Shared.AuthenticationDtos;

namespace Services
{
    public class AuthenticationService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, 
                                       IUnitOfWork unitOfWork, IMapper mapper) : IAuthenticationService
    {
        public async Task<string> PatientSignupService(PatientRegistrationModel patientRegistrationModel)
        {
            AppUser user = mapper.Map<Patient>(patientRegistrationModel);

            return await CreateUser(user, patientRegistrationModel.Password, patientRegistrationModel.Role);
        }

        public async Task<string> DoctorSignupService(DoctorRegistrationModel doctorRegistrationModel)
        {
            AppUser user = mapper.Map<Doctor>(doctorRegistrationModel);

            return await CreateUser(user, doctorRegistrationModel.Password, doctorRegistrationModel.Role);
        }

        public async Task<string> AdminSignupService(AdminRegistrationModel adminRegistrationModel)
        {
            AppUser user = mapper.Map<Admin>(adminRegistrationModel);

            return await CreateUser(user, adminRegistrationModel.Password, adminRegistrationModel.Role);
        }

        private async Task<string> CreateUser(AppUser user, string password, string role)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                //if (!roleResult.Succeeded)
                //{
                //    throw new Exception("Patient ID claim not found in token.");
                //}
            }

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                //throw new Exception($"{result.Errors.Select(e => e.Description).ToList()}");
                return "User is already exist";
            }

            var roleAssignResult = await userManager.AddToRoleAsync(user, role);
            //if (!roleAssignResult.Succeeded)
            //{
            //    throw new Exception($"{roleAssignResult.Errors.Select(e => e.Description)}");
            //}

            return "User created successfully.";
        }

        public async Task<object> SignInUserService(LoginModel loginModel, IOptions<AppSettings> appSettings)
        {
            var user = await userManager.FindByEmailAsync(loginModel.Email);

            if (user != null && await userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var roles = await userManager.GetRolesAsync(user);

                // Generate Token:
                var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Value.JWTSecret)); // get sign in key from the appsettings
                // Describing the claims that we want to include in the JWT payload:
                ClaimsIdentity claims = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserID", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, roles.First()),
                });

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddDays(3),
                    SigningCredentials = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256Signature), // Signing the token with the key:
                };
                // Creating the token:
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);

                await unitOfWork.SaveChangesAsync();

                return new { token };
            }
            else
            {
                throw new Exception("Invalid email or password!");
            }
        }

        public async Task<bool> CheckEmailService(CheckEmailRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);

            return user != null;
        }
    }
}
