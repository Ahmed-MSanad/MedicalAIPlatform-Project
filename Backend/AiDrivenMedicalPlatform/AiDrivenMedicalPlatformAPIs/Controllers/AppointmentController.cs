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
            var schedule = await _context.DoctorSchedules
                                 .Where(s => s.DoctorId == id)
                                 .OrderBy(s => s.Day)
                                 .Select(s => new ScheduleDto
                                 {
                                     Day = s.Day,
                                     From = s.From.ToString(),
                                     To = s.To.ToString()
                                 })
                                 .ToListAsync();

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
                                                   DoctorSchedule = schedule
                                               })
                                               .FirstOrDefaultAsync();
            if (doctor == null)
            {
                return NotFound(new { Message = "Doctor not found." });
            }
            return Ok(doctor);
        }

        [Authorize]
        [HttpPatch]
        public async Task<ActionResult> AddRate([FromQuery]string id, [FromQuery] int appointmentId, [FromQuery]int rate)
        {
            var doctor = _context.Doctors.Where(d => d.Id == id).FirstOrDefault();
            var appointment = _context.Appointments.Where(a => a.AppointmentId == appointmentId).FirstOrDefault();
            if (doctor == null)
            {
                return NotFound(new { Message = "Doctor not found." });
            }
            if(appointment == null)
            {
                return NotFound(new { Message = "Appointment not Found." });
            }
            if (rate < 1 || rate > 5)
            {
                return BadRequest(new { Message = "Rating Should Be from 1 to 5" });
            }

            doctor.TotalRating += rate;
            doctor.NumberOfRaters++;
            doctor.Rate = (decimal) doctor.TotalRating / doctor.NumberOfRaters;
            appointment.IsRated = true;
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
        [HttpDelete]
        public async Task<ActionResult> CancelAppointment([FromQuery] int appointmentId)
        {
           var appointment = await _context.Appointments.Where(appointment => appointment.AppointmentId == appointmentId).FirstOrDefaultAsync();
           if(appointment is null)
            {
                return NotFound(new { Message = "Appointment Not Found." });
            }
            try
            {
                _context.Appointments.Remove(appointment);
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
            if(appointment.Date < DateTime.UtcNow)
            {
                return BadRequest(new { Message = "Can't complete an appointment before its time" });
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
        public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments([FromQuery] int status)
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;

            var appointments = await _context.Appointments.Where(appointment =>
                                                                (appointment.Did == userId || appointment.Pid == userId) &&
                                                                appointment.Status == (AppointmentStatus)status)
                                                          .Include(a => a.Patient)
                                                          .Include(a => a.Doctor)
                                                          .Select(appointment => new AppointmentDto
                                                            {
                                                              Id = appointment.AppointmentId,
                                                                Date = appointment.Date,
                                                                Cost = appointment.Cost,
                                                                Location = appointment.Location,
                                                                PatientName = appointment.Patient.FullName,
                                                                DoctorName = appointment.Doctor.FullName,
                                                                IsRated = appointment.IsRated,
                                                                Did = appointment.Did,
                                                            })
                                                          .ToListAsync();


            return Ok(appointments);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<AppointmentInfoDto>> GetAppointmentInfo([FromQuery] int id)
        {
            var appointment = await _context.Appointments.Where(appointment => appointment.AppointmentId == id)
                                                          .Include(a => a.Patient)
                                                          .Include(a => a.Doctor)
                                                          .Select(appointment => new AppointmentInfoDto
                                                          {
                                                              Date = appointment.Date,
                                                              Cost = appointment.Cost,
                                                              Location = appointment.Location,
                                                              PatientName = appointment.Patient.FullName,
                                                              DoctorName = appointment.Doctor.FullName,
                                                              Description = appointment.Description,
                                                          })
                                                          .FirstOrDefaultAsync();
            if(appointment is null)
            {
                return NotFound(new { Message="Appointment not Found" });
            }
            return Ok(appointment);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAvailableTimeSlots([FromQuery] string id, [FromQuery] DateTime day)
        {
            string dayOfWeek = day.DayOfWeek.ToString();

            var schedule = await _context.DoctorSchedules
                                 .Where(s => s.DoctorId == id && s.Day == dayOfWeek)
                                 .FirstOrDefaultAsync();

            if (schedule == null)
            {
                return NotFound(new { Message = "Doctor has no schedule on this day." });
            }

            var slots = new List<TimeSpan>();
            var current = schedule.From;
            while (current + TimeSpan.FromMinutes(15) <= schedule.To)
            {
                slots.Add(current);
                current = current.Add(TimeSpan.FromMinutes(15));
            }

            var startOfDay = day.Date;
            var endOfDay = startOfDay.AddDays(1);

            var bookedSlots = await _context.Appointments
                                            .Where(a => a.Did == id && a.Date >= startOfDay && a.Date < endOfDay && a.Status == AppointmentStatus.Scheduled)
                                            .Select(a => a.Date.TimeOfDay)
                                            .ToListAsync();

            var availableSlots = slots.Except(bookedSlots).OrderBy(t => t).ToList();

            if (day.Date == DateTime.Today)
            {
                var currentTime = DateTime.Now.TimeOfDay;
                availableSlots = availableSlots.Where(slot => slot > currentTime).ToList();
            }

            return Ok(availableSlots);
        }

    }
}
