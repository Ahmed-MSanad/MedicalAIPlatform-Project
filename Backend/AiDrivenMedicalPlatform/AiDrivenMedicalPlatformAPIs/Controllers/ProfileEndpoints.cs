using System.Security.Claims;
using AiDrivenMedicalPlatformAPIs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    public static class ProfileEndpoints
    {
        public static IEndpointRouteBuilder MapProfileEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/profile", GetUserProfile);
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
                    Occupation = userDetails?.Occupation,
                    EmergencyContactName = userDetails?.EmergencyContactName,
                    EmergencyContactNumber = userDetails?.EmergencyContactNumber,
                    FamilyMedicalHistory = userDetails?.FamilyMedicalHistory,
                    PastMedicalHistory = userDetails?.PastMedicalHistory,
                }
            );
        }
    }
}
