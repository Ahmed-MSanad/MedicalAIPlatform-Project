using AutoMapper;
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction;
using Services.Specifications;
using Shared.AppointmentDtos;
using Shared.DoctorDtos;

namespace Services
{
    public class AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager) : IAppointmentService
    {
        public async Task<IEnumerable<DoctorResponseDto>> GetDoctorsInfoService(DoctorSpecificationParams specificationParams)
        {
            var specification = new DoctorWithFilterSpecification(specificationParams);

            var doctors = await unitOfWork.GetRepository<Doctor, string>().GetAllAsync(specification);

            if (doctors == null || !doctors.Any())
            {
                return Enumerable.Empty<DoctorResponseDto>();
            }

            var doctorsResponseDtos = mapper.Map<IEnumerable<DoctorResponseDto>>(doctors);

            return doctorsResponseDtos;
        }

        public async Task<DoctorInfoDto> GetDoctorInfoService(string id)
        {
            var doctorSpecification = new DoctorWithFilterSpecification(id);

            var doctor = await unitOfWork.GetRepository<Doctor, string>().GetByIdAsync(doctorSpecification);
                
            if (doctor == null)
            {
                throw new Exception("Doctor not found in Get Doctor Info Service");
            }

            var doctorInfoDto = mapper.Map<DoctorInfoDto>(doctor);

            return doctorInfoDto;
        }

        public async Task AddRateService(string id, int appointmentId, int rate)
        {
            var specification = new DoctorWithFilterSpecification(id);
            var doctor = await unitOfWork.GetRepository<Doctor, string>().GetByIdAsync(specification);
            if (doctor == null)
            {
                throw new Exception("Doctor not found in Add Rate Service");
            }

            var appointment = await unitOfWork.GetRepository<Appointment, int>().GetByIdAsync(appointmentId);
            if (appointment == null)
            {
                throw new Exception("Appointment not Found in Add Rate Service.");
            }

            doctor.TotalRating += rate;
            doctor.NumberOfRaters++;
            doctor.Rate = (decimal) doctor.TotalRating / doctor.NumberOfRaters;
            appointment.IsRated = true;

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<int> CreateAppointmentService(CreatedAppointmentDto appointmentDto, string patientId)
        {
            var appointment = mapper.Map<Appointment>(appointmentDto);

            appointment.Pid = patientId;

            await unitOfWork.GetRepository<Appointment, int>().AddAsync(appointment);

            await unitOfWork.SaveChangesAsync();

            return appointment.AppointmentId;
        }

        public async Task CancelAppointmentService(int appointmentId)
        {
            var appointment = await unitOfWork.GetRepository<Appointment, int>().GetByIdAsync(appointmentId);
            if (appointment is null)
            {
                throw new Exception("Appointment Not Found in Cancel Appointment Service");
            }

            unitOfWork.GetRepository<Appointment, int>().Delete(appointment);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task CompleteAppointmentService(int appointmentId)
        {
            var appointment = await unitOfWork.GetRepository<Appointment, int>().GetByIdAsync(appointmentId);
            if (appointment is null)
            {
                throw new Exception("Appointment Not Found in Complete Appointment Service");
            }
            if (appointment.Date > DateTime.UtcNow)
            {
                throw new Exception("Can't complete an appointment before its time");
            }
            
            appointment.Status = AppointmentStatus.Completed;

            unitOfWork.GetRepository<Appointment, int>().Update(appointment);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<AppointmentDto>> GetAppointmentsService(int status, string userId)
        {
            var specification = new AppointmentWithFilterSpecification(status, userId);

            var appointments = await unitOfWork.GetRepository<Appointment, int>().GetAllAsync(specification);

            if (appointments == null || !appointments.Any())
            {
                return Enumerable.Empty<AppointmentDto>();
            }

            var appointmentsDto = mapper.Map<IEnumerable<AppointmentDto>>(appointments);

            return appointmentsDto;
        }

        public async Task<List<TimeSpan>> GetAvailableTimeSlotsService(string id, DateTime day)
        {
            string dayOfWeek = day.DayOfWeek.ToString();

            var specification = new ScheduleWithFilterSpecification(id, dayOfWeek);

            var schedule = await unitOfWork.GetRepository<DoctorSchedule, string>().GetByIdAsync(specification);
            if (schedule == null)
            {
                throw new Exception("Schedule not found for the given doctor and day in Get Available Time Slots Service");
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

            var appointmentSpecification = new AppointmentWithFilterSpecification(id, startOfDay, endOfDay);
            
            var bookedSlots = (await unitOfWork.GetRepository<Appointment, int>().GetAllAsync(appointmentSpecification)).Select(a => a.Date.TimeOfDay);

            var availableSlots = slots.Except(bookedSlots).OrderBy(t => t).ToList();

            if (day.Date == DateTime.Today)
            {
                var currentTime = DateTime.Now.TimeOfDay;
                availableSlots = availableSlots.Where(slot => slot > currentTime).ToList();
            }

            return availableSlots;
        }

        public async Task<AppointmentInfoDto> GetAppointmentInfoService(int appointmentId)
        {

            var specification = new AppointmentWithFilterSpecification(appointmentId);

            var appointment = await unitOfWork.GetRepository<Appointment, int>().GetByIdAsync(specification);
            
            if(appointment is null)
            {
                throw new Exception("Appointment Not Found.");
            }

            var appointmentInfo = mapper.Map<AppointmentInfoDto>(appointment);

            return appointmentInfo;
        }
    }
}
