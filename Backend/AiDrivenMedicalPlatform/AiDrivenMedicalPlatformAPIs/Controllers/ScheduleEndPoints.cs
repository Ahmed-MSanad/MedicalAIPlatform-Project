using AiDrivenMedicalPlatformAPIs.Models;
using AiDrivenMedicalPlatformAPIs.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    public static class ScheduleEndPoints
    {
        public static IEndpointRouteBuilder MapScheduleEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/schedule", GetDoctorSchedule);
            app.MapPut("/schedule", EditDoctorSchedule);
            return app;
        }
        [Authorize]
        private static async Task<IResult> GetDoctorSchedule(ClaimsPrincipal user,AppDbContext context)
        {
            string userId = user.Claims.First(x => x.Type == "UserID").Value;
            var schedule = await context.DoctorSchedules.Where(s => s.DoctorId == userId).Select(x=> new { x.Day,x.From,x.To}).ToListAsync();
            return Results.Ok(schedule);

        }
        [Authorize]
        private static async Task<IResult> EditDoctorSchedule(ClaimsPrincipal user,AppDbContext context, [FromBody] List<ScheduleDto> schedules)
        {
            string userId = user.Claims.First(x => x.Type == "UserID").Value;
            var oldSchedule = context.DoctorSchedules.Where(s => s.DoctorId == userId);
            context.DoctorSchedules.RemoveRange(oldSchedule);
            var newSchedule = schedules.Select(day => new DoctorSchedule
            { 
            DoctorId = userId,
            Day = day.Day,
            From = TimeSpan.Parse(day.From),
            To = TimeSpan.Parse(day.To),
            }).ToList();
            context.DoctorSchedules.AddRange(newSchedule);
            await context.SaveChangesAsync();
            return Results.Ok("Schedule Updated Successfully");
        }
    }
}
