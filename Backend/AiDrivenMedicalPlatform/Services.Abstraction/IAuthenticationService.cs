using Microsoft.Extensions.Options;
using Shared;
using Shared.AuthenticationDtos;

namespace Services.Abstraction
{
    public interface IAuthenticationService
    {
        public Task<string> PatientSignupService(PatientRegistrationModel patientRegistrationModel);
        public Task<string> DoctorSignupService(DoctorRegistrationModel doctorRegistrationModel);
        public Task<string> AdminSignupService(AdminRegistrationModel adminRegistrationModel);
        public Task<object> SignInUserService(LoginModel loginModel, IOptions<AppSettings> appSettings);
        public Task<bool> CheckEmailService(CheckEmailRequest request);
    }
}
