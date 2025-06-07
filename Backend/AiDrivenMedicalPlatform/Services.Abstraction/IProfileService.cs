using shared.AdminDtos;
using Shared.DoctorDtos;
using Shared.PatientDtos;

namespace Services.Abstraction
{
    public interface IProfileService
    {
        public Task<PatientDto> GetPatientProfileService(string patientId);
        public Task<DoctorDto> GetDoctorProfileService(string doctorId);
        public Task<AdminDto> GetAdminProfileService(string adminId);
        public Task DeleteUserProfileService(string userId);
        public Task EditPatientProfileService(PatientDto updateRequest, string patientId);
        public Task EditDoctorProfileService(DoctorDto updateRequest, string doctorId);
        public Task EditAdminProfileService(AdminDto updateRequest, string adminId);
    }
}
