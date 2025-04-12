using AiDrivenMedicalPlatformAPIs.Models.Dto;
using MedicalProj.Data.Contexts;
using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    public static class ScheduleEndPoints
    {
        //public static IEndpointRouteBuilder MapScheduleEndpoints(this IEndpointRouteBuilder app)
        //{
        //    app.MapGet("/schedule", GetDoctorSchedule);
        //    app.MapPut("/schedule", EditDoctorSchedule);
        //    return app;
        //}
        //[Authorize]
        //private static async Task<IResult> GetDoctorSchedule(ClaimsPrincipal user,MedicalDbContext context)
        //{
        //    string userId = user.Claims.First(x => x.Type == "UserID").Value;
        //    var schedule = await context.DoctorSchedules.Where(s => s.DoctorId == userId).ToListAsync();
        //    return Results.Ok(schedule);

        //}
        //[Authorize]
        //private static async Task<IResult> EditDoctorSchedule(ClaimsPrincipal user, MedicalDbContext context, [FromBody] List<ScheduleDto> schedules)
        //{
        //    string userId = user.Claims.First(x => x.Type == "UserID").Value;
        //    var oldSchedule = context.DoctorSchedules.Where(s => s.DoctorId == userId);
        //    context.DoctorSchedules.RemoveRange(oldSchedule);
        //    var newSchedule = schedules.Select(day => new DoctorSchedule
        //    { 
        //    DoctorId = userId,
        //    Day = day.Day,
        //    From = day.From,
        //    To = day.To,
        //    }).ToList();
        //    context.DoctorSchedules.AddRange(newSchedule);
        //    await context.SaveChangesAsync();
        //    return Results.Ok("Schedule Updated Successfully");
        //}
    }
}
