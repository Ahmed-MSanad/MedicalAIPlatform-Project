using AiDrivenMedicalPlatformAPIs.Models.Dto;
using Azure.Core;
using MedicalProj.Data.Contexts;
using MedicalProj.Data.Models;
using MedicalProj.Data.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly MedicalDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public AppointmentController(MedicalDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorResponseDto>>> GetDoctorsInfo([FromQuery] DoctorFilterDto filterDto)
        {
            var doctors = _context.Doctors.AsQueryable();

            if (!string.IsNullOrEmpty(filterDto.Name))
            {
                doctors = doctors.Where(d => d.FullName.ToLower().Contains(filterDto.Name.ToLower()));
            }

            if (!string.IsNullOrEmpty(filterDto.Speciality))
            {
                doctors = doctors.Where(d => d.Specialisation.ToLower().Contains(filterDto.Speciality.ToLower()));
            }

            if (!string.IsNullOrEmpty(filterDto.Workplace))
            {
                doctors = doctors.Where(d => d.WorkPlace.ToLower().Contains(filterDto.Workplace.ToLower()));
            }

            if (filterDto.MinRate.HasValue)
            {
                doctors = doctors.Where(d => d.Rate >= filterDto.MinRate);
            }

            if (filterDto.Cost.HasValue)
            {
                doctors = doctors.Where(d=> d.Fee <=  filterDto.Cost);
            }

            return Ok(await doctors.Select(d=> new DoctorResponseDto
            {
                Id = d.Id,
                FullName = d.FullName,
                Specialisation = d.Specialisation,
                Rate = d.Rate,
                Fee = d.Fee,
                WorkPlace = d.WorkPlace,
                Image = d.Image
            }).ToListAsync());
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<DoctorInfoDto>> GetDoctorInfo([FromQuery] string id)
        {
            var doctor = await _context.Doctors.Include(d => d.DoctorPhones)
                                               .Include(d => d.DoctorSchedule)
                                               .Where(d => d.Id == id)
                                               .Select(d => new DoctorInfoDto
                                               {
                                                   FullName = d.FullName,
                                                   Specialisation = d.Specialisation,
                                                   WorkPlace = d.WorkPlace,
                                                   Fee = d.Fee,
                                                   Rate = d.Rate,
                                                   Image = d.Image,
                                                   DoctorPhones = d.DoctorPhones.Select(p => p.Phone).ToList(),
                                                   DoctorSchedule = new DoctorScheduleDto
                                                   {
                                                       Day = d.DoctorSchedule.Day,
                                                       From = d.DoctorSchedule.From,
                                                       To = d.DoctorSchedule.To,
                                                   }
                                               })
                                               .FirstOrDefaultAsync();
            if (doctor == null)
            {
                return NotFound(new { Message = "Doctor not found." });
            }
            return Ok(doctor);
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> AddRate([FromQuery]string id, [FromBody]int rate)
        {
            var doctor = _context.Doctors.Where(d => d.Id == id).FirstOrDefault();
            if (doctor == null)
            {
                return NotFound(new { Message = "Doctor not found." });
            }

            if (rate < 1 || rate > 5)
            {
                return BadRequest(new { Message = "Rating Should Be from 1 to 5" });
            }

            doctor.TotalRating += rate;
            doctor.Rate = rate;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new
                {
                    Message = "Rating added successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Failed to update rating.", Error = ex.Message });
            }

        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateAppointment([FromBody] CreatedAppointmentDto appointmentDto)
        {
            string patientId = User.Claims.First(x => x.Type == "UserID").Value;

            var appointment = new Appointment
            {
                Date = appointmentDto.Date,
                Cost = appointmentDto.Cost,
                Location = appointmentDto.Location,
                Description = appointmentDto.Description,
                Did = appointmentDto.Did,
                Pid = patientId
            };

            try
            {
                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                return StatusCode(201, new { Message = "Appointment created successfully" });
            }
            catch(Exception ex) 
            {
                return BadRequest(ex.Message);
            }    
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult> CancelAppointment([FromQuery] int appointmentId)
        {
           var appointment = await _context.Appointments.Where(appointment => appointment.AppointmentId == appointmentId).FirstOrDefaultAsync();
           if(appointment is null)
            {
                return NotFound(new { Message = "Appointment Not Found." });
            }
           appointment.CreatedAt = DateTime.UtcNow;
           appointment.Status = AppointmentStatus.Cancelled;
            try
            {
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();

                return Ok( new { Message = "Appointment Cancelled Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPatch]
        public async Task<ActionResult> CompleteAppointment([FromQuery] int appointmentId)
        {
            var appointment = await _context.Appointments.Where(appointment => appointment.AppointmentId == appointmentId).FirstOrDefaultAsync();
            if (appointment is null)
            {
                return NotFound(new { Message = "Appointment Not Found." });
            }
            appointment.Status = AppointmentStatus.Completed;
            try
            {
                _context.Appointments.Update(appointment);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Appointment Completed Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments()
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;

            var appointments = _context.Appointments.Where(appointment => (appointment.Did == userId ||  appointment.Pid == userId) && (appointment.Status == AppointmentStatus.Scheduled))
                                                    .Select(appointment => new AppointmentDto
                                                    {
                                                        Date = appointment.Date,
                                                        Cost = appointment.Cost,
                                                        Location = appointment.Location,
                                                        PatientName = appointment.Patient.FullName,
                                                        DoctorName = appointment.Doctor.FullName,
                                                    })
                                                    .ToListAsync();
            
            return Ok(appointments);
        }
        
    }
}
