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

            app.MapPost("/signup", CreateUser).AllowAnonymous();



            return app;
        }

        private static async Task<IResult> CreateUser([FromBody] UserRegistrationModel userRegistrationModel,
                                                                    UserManager<AppUser> userManager,
                                                                    RoleManager<IdentityRole> roleManager)
        {
            if (userRegistrationModel == null)
            {
                return Results.BadRequest("Invalid registration data.");
            }

            // Validate common properties
            if (string.IsNullOrEmpty(userRegistrationModel.Email) ||
                string.IsNullOrEmpty(userRegistrationModel.Password) ||
                string.IsNullOrEmpty(userRegistrationModel.FullName))
            {
                return Results.BadRequest("Email, password, and full name are required.");
            }

            AppUser user;

            switch (userRegistrationModel)
            {
                case PatientRegistrationModel patientModel:
                    if (string.IsNullOrEmpty(patientModel.Occupation) ||
                        string.IsNullOrEmpty(patientModel.EmergencyContactName))
                    {
                        return Results.BadRequest("Occupation and emergency contact name are required for patients.");
                    }

                    user = new Patient
                    {
                        UserName = patientModel.Email,
                        Email = patientModel.Email,
                        FullName = patientModel.FullName,
                        PhoneNumber = patientModel.PhoneNumber,
                        DateOfBirth = patientModel.DateOfBirth,
                        Gender = patientModel.Gender,
                        Address = patientModel.Address,
                        Occupation = patientModel.Occupation,
                        EmergencyContactName = patientModel.EmergencyContactName,
                        EmergencyContactNumber = patientModel.EmergencyContactNumber,
                        FamilyMedicalHistory = patientModel.FamilyMedicalHistory,
                        PastMedicalHistory = patientModel.PastMedicalHistory
                    };
                    break;

                case DoctorRegistrationModel doctorModel:
                    if (string.IsNullOrEmpty(doctorModel.MedicalLicenseNumber) ||
                        string.IsNullOrEmpty(doctorModel.Specialisation))
                    {
                        return Results.BadRequest("Medical license number and specialisation are required for doctors.");
                    }

                    user = new Doctor
                    {
                        UserName = doctorModel.Email,
                        Email = doctorModel.Email,
                        FullName = doctorModel.FullName,
                        PhoneNumber = doctorModel.PhoneNumber,
                        DateOfBirth = doctorModel.DateOfBirth,
                        Gender = doctorModel.Gender,
                        Address = doctorModel.Address,
                        IdentificationNumber = doctorModel.IdentificationNumber,
                        MedicalLicenseNumber = doctorModel.MedicalLicenseNumber,
                        Specialisation = doctorModel.Specialisation,
                        WorkPlace = doctorModel.WorkPlace
                    };
                    break;

                case AdminRegistrationModel adminModel:
                    if (string.IsNullOrEmpty(adminModel.MedicalLicenseNumber) ||
                        string.IsNullOrEmpty(adminModel.Specialisation))
                    {
                        return Results.BadRequest("Medical license number and specialisation are required for admins.");
                    }

                    user = new Admin
                    {
                        UserName = adminModel.Email,
                        Email = adminModel.Email,
                        FullName = adminModel.FullName,
                        PhoneNumber = adminModel.PhoneNumber,
                        DateOfBirth = adminModel.DateOfBirth,
                        Gender = adminModel.Gender,
                        Address = adminModel.Address,
                        IdentificationNumber = adminModel.IdentificationNumber,
                        MedicalLicenseNumber = adminModel.MedicalLicenseNumber,
                        Specialisation = adminModel.Specialisation
                    };
                    break;

                default:
                    return Results.BadRequest("Invalid role specified.");
            }

            // Ensure the role exists
            var roleExists = await roleManager.RoleExistsAsync(userRegistrationModel.Role);
            if (!roleExists)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(userRegistrationModel.Role));
                if (!roleResult.Succeeded)
                {
                    return Results.BadRequest("Failed to create role.");
                }
            }

            // Create the user
            var result = await userManager.CreateAsync(user, userRegistrationModel.Password);
            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors.Select(e => e.Description));
            }

            // Assign the role
            var roleAssignResult = await userManager.AddToRoleAsync(user, userRegistrationModel.Role);
            if (!roleAssignResult.Succeeded)
            {
                return Results.BadRequest(roleAssignResult.Errors.Select(e => e.Description));
            }

            return Results.Ok(new { Message = "User created successfully.", UserId = user.Id });
        }
        
    }

    // Concrete base class for registration models
    public abstract class UserRegistrationModel
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
    }

    [JsonDerivedType(typeof(PatientRegistrationModel), "Patient")]
    [JsonDerivedType(typeof(DoctorRegistrationModel), "Doctor")]
    [JsonDerivedType(typeof(AdminRegistrationModel), "Admin")]
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
