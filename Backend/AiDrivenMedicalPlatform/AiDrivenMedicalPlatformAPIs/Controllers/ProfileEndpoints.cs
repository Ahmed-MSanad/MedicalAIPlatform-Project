using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    public static class ProfileEndpoints
    {
        public static IEndpointRouteBuilder MapProfileEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/profile/patient", GetPatientProfile);
            app.MapPut("/profile/patient", EditPatientProfile);

            app.MapGet("/profile/doctor", GetDoctorProfile);
            app.MapPut("/profile/doctor", EditDoctorProfile);

            app.MapGet("/profile/admin", GetAdminProfile);
            app.MapPut("/profile/admin", EditAdminProfile);

            app.MapDelete("/profile", DeleteUserProfile);
            return app;
        }
        [Authorize]
        private static async Task<IResult> GetPatientProfile(ClaimsPrincipal user, UserManager<Patient> userManager)
        {
            string userId = user.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await userManager.FindByIdAsync(userId);
            return Results.Ok(
                new
                {
                    Email = userDetails?.Email,
                    FullName = userDetails?.FullName,
                    DateOfBirth = userDetails?.DateOfBirth,
                    Gender = userDetails?.Gender,
                    Address = userDetails?.Address,
                    Phone = userDetails?.PhoneNumber,
                    Occupation = userDetails?.Occupation,
                    EmergencyContactName = userDetails?.EmergencyContactName,
                    EmergencyContactNumber = userDetails?.EmergencyContactNumber,
                    FamilyMedicalHistory = userDetails?.FamilyMedicalHistory,
                    PastMedicalHistory = userDetails?.PastMedicalHistory,
                    Image = userDetails?.Image,
                }
            );
        }

        [Authorize]
        private static async Task<IResult> EditPatientProfile(ClaimsPrincipal user, UserManager<Patient> userManager, [FromBody] Patient updateRequest)
        {
            string userId = user.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await userManager.FindByIdAsync(userId);
            if (userDetails == null)
            {
                return Results.NotFound("User not found");
            }

            userDetails.FullName = updateRequest.FullName;
            userDetails.DateOfBirth = updateRequest.DateOfBirth;
            userDetails.Gender = updateRequest.Gender;
            userDetails.Address = updateRequest.Address;
            userDetails.PhoneNumber = updateRequest.PhoneNumber;
            userDetails.Occupation = updateRequest.Occupation;
            userDetails.EmergencyContactName = updateRequest.EmergencyContactName;
            userDetails.EmergencyContactNumber = updateRequest.EmergencyContactNumber;
            userDetails.FamilyMedicalHistory = updateRequest.FamilyMedicalHistory;
            userDetails.PastMedicalHistory = updateRequest.PastMedicalHistory;
            userDetails.Image = updateRequest.Image;
            var result = await userManager.UpdateAsync(userDetails);
            if (result.Succeeded)
            {
                return Results.Ok("Profile updated successfully");
            }
            return Results.BadRequest(result.Errors);

        }


        [Authorize]
        private static async Task<IResult> GetDoctorProfile(ClaimsPrincipal user, UserManager<Doctor> userManager)
        {
            string userId = user.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await userManager.FindByIdAsync(userId);
            return Results.Ok(
                new
                {
                    Email = userDetails?.Email,
                    FullName = userDetails?.FullName,
                    DateOfBirth = userDetails?.DateOfBirth,
                    Gender = userDetails?.Gender,
                    Address = userDetails?.Address,
                    Phone = userDetails?.PhoneNumber,
                    Image = userDetails?.Image,
                    IdentificationNumber = userDetails?.IdentificationNumber,
                    MedicalLicenseNumber = userDetails?.MedicalLicenseNumber,
                    Specialisation = userDetails?.Specialisation,
                    WorkPlace = userDetails?.WorkPlace,
                }
            );
        }

        [Authorize]
        private static async Task<IResult> EditDoctorProfile(ClaimsPrincipal user, UserManager<Doctor> userManager, [FromBody] Doctor updateRequest)
        {
            string userId = user.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await userManager.FindByIdAsync(userId);
            if (userDetails == null)
            {
                return Results.NotFound("User not found");
            }

            userDetails.FullName = updateRequest.FullName;
            userDetails.DateOfBirth = updateRequest.DateOfBirth;
            userDetails.Gender = updateRequest.Gender;
            userDetails.Address = updateRequest.Address;
            userDetails.PhoneNumber = updateRequest.PhoneNumber;
            userDetails.Image = updateRequest.Image;
            userDetails.IdentificationNumber = updateRequest.IdentificationNumber;
            userDetails.MedicalLicenseNumber = updateRequest.MedicalLicenseNumber;
            userDetails.Specialisation = updateRequest.Specialisation;
            userDetails.WorkPlace = updateRequest.WorkPlace;
            var result = await userManager.UpdateAsync(userDetails);
            if (result.Succeeded)
            {
                return Results.Ok("Profile updated successfully");
            }
            return Results.BadRequest(result.Errors);

        }


        [Authorize]
        private static async Task<IResult> GetAdminProfile(ClaimsPrincipal user, UserManager<Admin> userManager)
        {
            string userId = user.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await userManager.FindByIdAsync(userId);
            return Results.Ok(
                new
                {
                    Email = userDetails?.Email,
                    FullName = userDetails?.FullName,
                    DateOfBirth = userDetails?.DateOfBirth,
                    Gender = userDetails?.Gender,
                    Address = userDetails?.Address,
                    Phone = userDetails?.PhoneNumber,
                    Image = userDetails?.Image,
                    IdentificationNumber = userDetails?.IdentificationNumber,
                    MedicalLicenseNumber = userDetails?.MedicalLicenseNumber,
                    Specialisation = userDetails?.Specialisation,
                }
            );
        }

        [Authorize]
        private static async Task<IResult> EditAdminProfile(ClaimsPrincipal user, UserManager<Admin> userManager, [FromBody] Admin updateRequest)
        {
            string userId = user.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await userManager.FindByIdAsync(userId);
            if (userDetails == null)
            {
                return Results.NotFound("User not found");
            }

            userDetails.FullName = updateRequest.FullName;
            userDetails.DateOfBirth = updateRequest.DateOfBirth;
            userDetails.Gender = updateRequest.Gender;
            userDetails.Address = updateRequest.Address;
            userDetails.PhoneNumber = updateRequest.PhoneNumber;
            userDetails.Image = updateRequest.Image;
            userDetails.IdentificationNumber = updateRequest.IdentificationNumber;
            userDetails.MedicalLicenseNumber = updateRequest.MedicalLicenseNumber;
            userDetails.Specialisation = updateRequest.Specialisation;
            var result = await userManager.UpdateAsync(userDetails);
            if (result.Succeeded)
            {
                return Results.Ok("Profile updated successfully");
            }
            return Results.BadRequest(result.Errors);

        }


        [Authorize]
        private static async Task<IResult> DeleteUserProfile(ClaimsPrincipal user, UserManager<AppUser> userManager)
        {
            string userId = user.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await userManager.FindByIdAsync(userId);
            if (userDetails == null) return Results.NotFound("User not found");

            var result = await userManager.DeleteAsync(userDetails);
            if (result.Succeeded)
            {
                return Results.Ok("User deleted successfully");
            }
            else
            {
                return Results.BadRequest(result.Errors);
            }
        }
    }
}
