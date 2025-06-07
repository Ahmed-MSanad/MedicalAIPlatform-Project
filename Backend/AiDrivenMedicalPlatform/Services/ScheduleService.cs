using AutoMapper;
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;
using Services.Abstraction;
using Services.Specifications;
using Shared.ScheduleDtos;

namespace Services
{
    public class ScheduleService(IUnitOfWork unitOfWork, IMapper mapper) : IScheduleService
    {
        public async Task<IEnumerable<ScheduleDto>> GetDoctorScheduleService(string doctorId)
        {
            var specification = new ScheduleWithFilterSpecification(doctorId);

            var schedules = await unitOfWork.GetRepository<DoctorSchedule, string>().GetAllAsync(specification);

            if(schedules == null || schedules.Count() == 0)
            {
                throw new Exception($"No Schedules Found For this doctor");
            }

            var schedulesDtos = mapper.Map<IEnumerable<ScheduleDto>>(schedules);

            return schedulesDtos;
        }

        public async Task EditDoctorScheduleService(List<ScheduleDto> schedules, string doctorId)
        {
            var specification = new ScheduleWithFilterSpecification(doctorId);
            
            var oldSchedule = await unitOfWork.GetRepository<DoctorSchedule, string>().GetAllAsync(specification);

            unitOfWork.GetRepository<DoctorSchedule, string>().DeleteAll(oldSchedule);

            var newSchedule = schedules.Select(day => new DoctorSchedule
            {
                DoctorId = doctorId,
                Day = day.Day,
                From = TimeSpan.Parse(day.From),
                To = TimeSpan.Parse(day.To),
            }).ToList();

            await unitOfWork.GetRepository<DoctorSchedule, string> ().AddAllAsync(newSchedule);

            await unitOfWork.SaveChangesAsync();
        }
    }
}
