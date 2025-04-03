using AiDrivenMedicalPlatformAPIs.Models;
using AiDrivenMedicalPlatformAPIs.Models.Dto;
using AiDrivenMedicalPlatformAPIs.Types;
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
            app.MapGet("/profile", GetUserProfile);
            app.MapPut("/profile", EditUserProfile);
            app.MapDelete("/profile", DeleteUserProfile);
            return app;
        }
        [Authorize]
        private static async Task<IResult> GetUserProfile(ClaimsPrincipal user, UserManager<AppUser> userManager)
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


        [Authorize]
        private static async Task<IResult> EditUserProfile(
            ClaimsPrincipal user,
            UserManager<AppUser> userManager,
            [FromBody] AppUserDto updateRequest)
        {
            string userId = user.Claims.First(x => x.Type == "UserID").Value;
            var userDetails = await userManager.FindByIdAsync(userId);
            if (userDetails == null)
            {
                return Results.NotFound("User not found");
            }

            userDetails.FullName = updateRequest.FullName;
            userDetails.DateOfBirth = updateRequest.DateOfBirth;
            userDetails.Gender = (Gender)updateRequest.Gender;
            userDetails.Address = updateRequest.Address;
            userDetails.PhoneNumber = updateRequest.Phone;
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
    }
}
