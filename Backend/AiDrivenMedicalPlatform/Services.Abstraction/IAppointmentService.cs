using Shared.AppointmentDtos;
using Shared.DoctorDtos;

namespace Services.Abstraction
{
    public interface IAppointmentService
    {
        public Task<IEnumerable<DoctorResponseDto>> GetDoctorsInfoService(DoctorSpecificationParams specificationParams);
        public Task<DoctorInfoDto> GetDoctorInfoService(string id);
        public Task AddRateService(string id, int appointmentId, int rate);
        public Task<int> CreateAppointmentService(CreatedAppointmentDto appointmentDto, string patientId);
        public Task CancelAppointmentService(int appointmentId);
        public Task CompleteAppointmentService(int appointmentId);
        public Task<IEnumerable<AppointmentDto>> GetAppointmentsService(int status, string userId);
        public Task<List<TimeSpan>> GetAvailableTimeSlotsService(string id, DateTime day);
        public Task<AppointmentInfoDto> GetAppointmentInfoService (int appointmentId);
    }
}
