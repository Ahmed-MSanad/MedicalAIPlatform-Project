using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MedicalProj.Data.Types;
using AiDrivenMedicalPlatformAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    public static class IdentityUserEndPoints
    {
        public static IEndpointRouteBuilder MapIdentityUserEndPoints(this IEndpointRouteBuilder app)
        {

            app.MapPost("/signup/patient", CreatePatient).AllowAnonymous();
            app.MapPost("/signup/doctor", CreateDoctor).AllowAnonymous();
            app.MapPost("/signup/admin", CreateAdmin).AllowAnonymous();

            app.MapPost("/signin", SignInUser);

            return app;
        }

        private static async Task<IResult> CreatePatient([FromBody] PatientRegistrationModel patientRegistrationModel,
                                                                    UserManager<AppUser> userManager,
                                                                    RoleManager<IdentityRole> roleManager)
        {
            if (patientRegistrationModel == null)
            {
                return Results.BadRequest("Invalid registration data.");
            }

            // Validate common properties
            if (string.IsNullOrEmpty(patientRegistrationModel.Email) ||
                string.IsNullOrEmpty(patientRegistrationModel.Password) ||
                string.IsNullOrEmpty(patientRegistrationModel.FullName))
            {
                return Results.BadRequest("Email, password, and full name are required.");
            }

            if (string.IsNullOrEmpty(patientRegistrationModel.Occupation) ||
                string.IsNullOrEmpty(patientRegistrationModel.EmergencyContactName))
            {
                return Results.BadRequest("Occupation and emergency contact name are required for patients.");
            }

            AppUser user;
            user = new Patient
            {
                UserName = patientRegistrationModel.Email,
                Email = patientRegistrationModel.Email,
                FullName = patientRegistrationModel.FullName,
                DateOfBirth = patientRegistrationModel.DateOfBirth,
                Gender = patientRegistrationModel.Gender,
                Address = patientRegistrationModel.Address,
                Occupation = patientRegistrationModel.Occupation,
                EmergencyContactName = patientRegistrationModel.EmergencyContactName,
                EmergencyContactNumber = patientRegistrationModel.EmergencyContactNumber,
                FamilyMedicalHistory = patientRegistrationModel.FamilyMedicalHistory,
                PastMedicalHistory = patientRegistrationModel.PastMedicalHistory,
                PatientPhones = patientRegistrationModel.Phones.Select(phone => new PatientPhone
                {
                    Phone = phone,
                }).ToList(),
                PhoneNumber = patientRegistrationModel.Phones.FirstOrDefault(),
            };
            return await CreateUser(user, patientRegistrationModel.Password, patientRegistrationModel.Role, userManager, roleManager);
        }


        private static async Task<IResult> CreateDoctor([FromBody] DoctorRegistrationModel doctorRegistrationModel,
                                                                    UserManager<AppUser> userManager,
                                                                    RoleManager<IdentityRole> roleManager)
        {
            if (doctorRegistrationModel == null)
            {
                return Results.BadRequest("Invalid registration data.");
            }
            if (string.IsNullOrEmpty(doctorRegistrationModel.Email) ||
                string.IsNullOrEmpty(doctorRegistrationModel.Password) ||
                string.IsNullOrEmpty(doctorRegistrationModel.FullName))
            {
                return Results.BadRequest("Email, password, and full name are required.");
            }

            if (string.IsNullOrEmpty(doctorRegistrationModel.MedicalLicenseNumber) ||
                string.IsNullOrEmpty(doctorRegistrationModel.Specialisation))
            {
                return Results.BadRequest("Medical License number and Specialisation name are required for doctors.");
            }

            AppUser user;
            user = new Doctor
            {
                UserName = doctorRegistrationModel.Email,
                Email = doctorRegistrationModel.Email,
                FullName = doctorRegistrationModel.FullName,
                DateOfBirth = doctorRegistrationModel.DateOfBirth,
                Gender = doctorRegistrationModel.Gender,
                Address = doctorRegistrationModel.Address,
                IdentificationNumber = doctorRegistrationModel.IdentificationNumber,
                MedicalLicenseNumber = doctorRegistrationModel.MedicalLicenseNumber,
                Specialisation = doctorRegistrationModel.Specialisation,
                WorkPlace = doctorRegistrationModel.WorkPlace,
                DoctorPhones = doctorRegistrationModel.Phones.Select(phone => new DoctorPhone
                {
                    Phone = phone,
                }).ToList(),
                PhoneNumber = doctorRegistrationModel.Phones.FirstOrDefault(),
            };
            return await CreateUser(user, doctorRegistrationModel.Password, doctorRegistrationModel.Role, userManager, roleManager);
        }

        private static async Task<IResult> CreateAdmin([FromBody] AdminRegistrationModel adminRegistrationModel,
                                                                    UserManager<AppUser> userManager,
                                                                    RoleManager<IdentityRole> roleManager)
        {
            if (adminRegistrationModel == null)
            {
                return Results.BadRequest("Invalid registration data.");
            }

            // Validate common properties
            if (string.IsNullOrEmpty(adminRegistrationModel.Email) ||
                string.IsNullOrEmpty(adminRegistrationModel.Password) ||
                string.IsNullOrEmpty(adminRegistrationModel.FullName))
            {
                return Results.BadRequest("Email, password, and full name are required.");
            }

            if (string.IsNullOrEmpty(adminRegistrationModel.MedicalLicenseNumber) ||
                string.IsNullOrEmpty(adminRegistrationModel.Specialisation))
            {
                return Results.BadRequest("Medical License number and Specialisation name are required for admins.");
            }

            AppUser user;
            user = new Admin
            {
                UserName = adminRegistrationModel.Email,
                Email = adminRegistrationModel.Email,
                FullName = adminRegistrationModel.FullName,
                DateOfBirth = adminRegistrationModel.DateOfBirth,
                Gender = adminRegistrationModel.Gender,
                Address = adminRegistrationModel.Address,
                IdentificationNumber = adminRegistrationModel.IdentificationNumber,
                MedicalLicenseNumber = adminRegistrationModel.MedicalLicenseNumber,
                Specialisation = adminRegistrationModel.Specialisation,
                AdminPhones = adminRegistrationModel.Phones.Select(phone => new AdminPhone
                {
                    Phone = phone,
                }).ToList(),
                PhoneNumber = adminRegistrationModel.Phones.FirstOrDefault(),
            };
            return await CreateUser(user, adminRegistrationModel.Password, adminRegistrationModel.Role, userManager, roleManager);
        }

        private static async Task<IResult> CreateUser(AppUser user, string password, string role,
                                                      UserManager<AppUser> userManager,
                                                      RoleManager<IdentityRole> roleManager)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                if (!roleResult.Succeeded)
                {
                    return Results.BadRequest("Failed to create role.");
                }
            }

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors.Select(e => e.Description));
            }

            var roleAssignResult = await userManager.AddToRoleAsync(user, role);
            if (!roleAssignResult.Succeeded)
            {
                return Results.BadRequest(roleAssignResult.Errors.Select(e => e.Description));
            }

            return Results.Ok(new { Message = "User created successfully." });
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
                //if(user.HospitalId != null)
                //{
                //    claims.AddClaim(new Claim("HospitalId", user.HospitalId.ToString()!));
                //}
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



        public class LoginModel
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

    }


    public abstract class UserRegistrationModel
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public ICollection<string> Phones { get; set; } = new List<string>();
    }

    
    public class PatientRegistrationModel : UserRegistrationModel
    {
        public string Occupation { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string FamilyMedicalHistory { get; set; }
        public string PastMedicalHistory { get; set; }
    }

    public class DoctorRegistrationModel : UserRegistrationModel
    {
        public string IdentificationNumber { get; set; }
        public string MedicalLicenseNumber { get; set; }
        public string Specialisation { get; set; }
        public string WorkPlace { get; set; }
    }

    public class AdminRegistrationModel : UserRegistrationModel
    {
        public string IdentificationNumber { get; set; }
        public string MedicalLicenseNumber { get; set; }
        public string Specialisation { get; set; }

    }
}
