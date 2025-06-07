using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.ScheduleDtos;

namespace Presentation.Controllers
{
    public class ScheduleController(IServiceManager serviceManager) : ApiController
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ScheduleDto>> GetDoctorSchedule()
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;

            var schedules = await serviceManager.ScheduleService.GetDoctorScheduleService(userId);

            return Ok(schedules);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> EditDoctorSchedule([FromBody] List<ScheduleDto> schedules)
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;

            await serviceManager.ScheduleService.EditDoctorScheduleService(schedules, userId);

            return Ok(new { Message = "Schedule Updated Successfully" });

        }
    }
}
