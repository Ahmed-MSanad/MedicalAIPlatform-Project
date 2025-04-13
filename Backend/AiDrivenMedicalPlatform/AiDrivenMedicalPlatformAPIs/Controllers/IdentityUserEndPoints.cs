using MedicalProj.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MedicalProj.Data.Types;
using AiDrivenMedicalPlatformAPIs.Models;
using System.Text.Json.Serialization;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    public static class IdentityUserEndPoints
    {
        public static IEndpointRouteBuilder MapIdentityUserEndPoints(this IEndpointRouteBuilder app)
        {

            app.MapPost("/signup/patient", CreatePatient).AllowAnonymous();
            app.MapPost("/signup/doctor", CreateDoctor).AllowAnonymous();
            app.MapPost("/signup/admin", CreateAdmin).AllowAnonymous();

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

            AppUser user;
            if (string.IsNullOrEmpty(patientRegistrationModel.Occupation) ||
                string.IsNullOrEmpty(patientRegistrationModel.EmergencyContactName))
            {
                return Results.BadRequest("Occupation and emergency contact name are required for patients.");
            }

                    user = new Patient
                    {
                        UserName = patientRegistrationModel.Email,
                        Email = patientRegistrationModel.Email,
                        FullName = patientRegistrationModel.FullName,
                        PatientPhones = patientRegistrationModel.PatientPhones,
                        DateOfBirth = patientRegistrationModel.DateOfBirth,
                        Gender = patientRegistrationModel.Gender,
                        Address = patientRegistrationModel.Address,
                        Occupation = patientRegistrationModel.Occupation,
                        EmergencyContactName = patientRegistrationModel.EmergencyContactName,
                        EmergencyContactNumber = patientRegistrationModel.EmergencyContactNumber,
                        FamilyMedicalHistory = patientRegistrationModel.FamilyMedicalHistory,
                        PastMedicalHistory = patientRegistrationModel.PastMedicalHistory
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

            AppUser user;
            if (string.IsNullOrEmpty(doctorRegistrationModel.MedicalLicenseNumber) ||
                string.IsNullOrEmpty(doctorRegistrationModel.Specialisation))
            {
                return Results.BadRequest("Medical License number and Specialisation name are required for doctors.");
            }

            user = new Doctor
            {
                UserName = doctorRegistrationModel.Email,
                Email = doctorRegistrationModel.Email,
                FullName = doctorRegistrationModel.FullName,
                DoctorPhones = doctorRegistrationModel.DoctorPhones,
                DateOfBirth = doctorRegistrationModel.DateOfBirth,
                Gender = doctorRegistrationModel.Gender,
                Address = doctorRegistrationModel.Address,
                IdentificationNumber = doctorRegistrationModel.IdentificationNumber,
                MedicalLicenseNumber = doctorRegistrationModel.MedicalLicenseNumber,
                Specialisation = doctorRegistrationModel.Specialisation,
                WorkPlace = doctorRegistrationModel.WorkPlace
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

            AppUser user;
            if (string.IsNullOrEmpty(adminRegistrationModel.MedicalLicenseNumber) ||
                string.IsNullOrEmpty(adminRegistrationModel.Specialisation))
            {
                return Results.BadRequest("Medical License number and Specialisation name are required for admins.");
            }

            user = new Admin
            {
                UserName = adminRegistrationModel.Email,
                Email = adminRegistrationModel.Email,
                FullName = adminRegistrationModel.FullName,
                AdminPhones = adminRegistrationModel.AdminPhones,
                DateOfBirth = adminRegistrationModel.DateOfBirth,
                Gender = adminRegistrationModel.Gender,
                Address = adminRegistrationModel.Address,
                IdentificationNumber = adminRegistrationModel.IdentificationNumber,
                MedicalLicenseNumber = adminRegistrationModel.MedicalLicenseNumber,
                Specialisation = adminRegistrationModel.Specialisation
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

            return Results.Ok(new { Message = "User created successfully.", UserId = user.Id });
        }


    }




    public abstract class UserRegistrationModel
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }

    
    public class PatientRegistrationModel : UserRegistrationModel
    {
        public ICollection<PatientPhones> PatientPhones { get; set; } = new List<PatientPhones>();
        public string Occupation { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string FamilyMedicalHistory { get; set; }
        public string PastMedicalHistory { get; set; }

    }

    public class DoctorRegistrationModel : UserRegistrationModel
    {
        public ICollection<DoctorPhones> DoctorPhones { get; set; } = new List<DoctorPhones>();
        public string IdentificationNumber { get; set; }
        public string MedicalLicenseNumber { get; set; }
        public string Specialisation { get; set; }
        public string WorkPlace { get; set; }
    }

    public class AdminRegistrationModel : UserRegistrationModel
    {

        public ICollection<AdminPhones> AdminPhones { get; set; } = new List<AdminPhones>();
        public string IdentificationNumber { get; set; }
        public string MedicalLicenseNumber { get; set; }
        public string Specialisation { get; set; }

    }
}
