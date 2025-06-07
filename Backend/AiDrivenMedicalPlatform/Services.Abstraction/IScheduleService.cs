using Shared.ScheduleDtos;

namespace Services.Abstraction
{
    public interface IScheduleService
    {
        public Task<IEnumerable<ScheduleDto>> GetDoctorScheduleService(string doctorId);
        public Task EditDoctorScheduleService(List<ScheduleDto> schedules, string doctorId);
    }
}
