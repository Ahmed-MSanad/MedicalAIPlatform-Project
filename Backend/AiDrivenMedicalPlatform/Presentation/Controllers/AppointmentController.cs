﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.AppointmentDtos;
using Shared.DoctorDtos;

namespace Presentation.Controllers
{
    public class AppointmentController(IServiceManager serviceManager) : ApiController
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorResponseDto>>> GetDoctorsInfo([FromQuery] DoctorSpecificationParams specificationParams)
        {
            var doctorResponseDtos = await serviceManager.AppointmentService.GetDoctorsInfoService(specificationParams);

            return Ok(doctorResponseDtos);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<DoctorInfoDto>> GetDoctorInfo([FromQuery] string id)
        {
            var doctorInfoDto = await serviceManager.AppointmentService.GetDoctorInfoService(id);

            return Ok(doctorInfoDto);
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult> AddRate([FromQuery]string id, [FromQuery] int appointmentId, [FromQuery]int rate)
        {
            if (rate < 1 || rate > 5)
            {
                return BadRequest(new { Message = "Rating Should Be from 1 to 5" });
            }

            await serviceManager.AppointmentService.AddRateService(id, appointmentId, rate);

            return Ok(new { Message = "Rating added successfully." });
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateAppointment([FromBody] CreatedAppointmentDto appointmentDto)
        {
            string patientId = User.Claims.First(x => x.Type == "UserID").Value;

            var appointmentId = await serviceManager.AppointmentService.CreateAppointmentService(appointmentDto, patientId);

            return StatusCode(StatusCodes.Status201Created, new { Message = "Appointment created successfully", AppointmentId = appointmentId});
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> CancelAppointment([FromQuery] int appointmentId)
        {
            await serviceManager.AppointmentService.CancelAppointmentService(appointmentId);

            return Ok(new { Message = "Appointment Cancelled Successfully" });
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult> CompleteAppointment([FromQuery] int appointmentId)
        {
            await serviceManager.AppointmentService.CompleteAppointmentService(appointmentId);

            return Ok(new { Message = "Appointment Completed Successfully" });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments([FromQuery] int status)
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;

            var appointments = await serviceManager.AppointmentService.GetAppointmentsService(status, userId);

            return Ok(appointments);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<AppointmentInfoDto>> GetAvailableTimeSlots([FromQuery] string id, [FromQuery] DateTime day)
        {
            var availableSlots = await serviceManager.AppointmentService.GetAvailableTimeSlotsService(id, day);

            return Ok(availableSlots);
        }
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<AppointmentInfoDto>> GetAppointmentInfo([FromQuery] int appointmentId)
        {
            var appointmentInfo = await serviceManager.AppointmentService.GetAppointmentInfoService(appointmentId);

            return Ok(appointmentInfo);
        }

    }
}
