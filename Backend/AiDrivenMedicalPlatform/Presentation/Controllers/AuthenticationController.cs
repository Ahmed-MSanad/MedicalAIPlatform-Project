using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Abstraction;
using Shared;
using Shared.AuthenticationDtos;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManager serviceManager) : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> PatientSignup([FromBody] PatientRegistrationModel patientRegistrationModel)
        {
            if (patientRegistrationModel == null)
            {
                return BadRequest(new { error = "Invalid registration data." });
            }

            if (string.IsNullOrEmpty(patientRegistrationModel.Email) || string.IsNullOrEmpty(patientRegistrationModel.Password) ||
                string.IsNullOrEmpty(patientRegistrationModel.FullName) || string.IsNullOrEmpty(patientRegistrationModel.Occupation) ||
                string.IsNullOrEmpty(patientRegistrationModel.EmergencyContactName))
            {
                return BadRequest(new { error = "Email, password, full name, Occupation and emergency contact name are required." });
            }

            string message = await serviceManager.AuthenticationService.PatientSignupService(patientRegistrationModel);

            return Ok(new { message });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> DoctorSignup([FromBody] DoctorRegistrationModel doctorRegistrationModel)
        {
            if (doctorRegistrationModel == null)
            {
                return BadRequest("Invalid registration data.");
            }
            if (string.IsNullOrEmpty(doctorRegistrationModel.Email) ||
                string.IsNullOrEmpty(doctorRegistrationModel.Password) ||
                string.IsNullOrEmpty(doctorRegistrationModel.FullName) ||
                string.IsNullOrEmpty(doctorRegistrationModel.MedicalLicenseNumber) ||
                string.IsNullOrEmpty(doctorRegistrationModel.Specialisation))
            {
                return BadRequest("Email, password, full name, Medical License number and Specialisation name are required.");
            }

            if (doctorRegistrationModel.Fee < 0)
            {
                return BadRequest("Medical Examination Fees must be 0 or more");
            }

            string message = await serviceManager.AuthenticationService.DoctorSignupService(doctorRegistrationModel);

            return Ok(new { message });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AdminSignup([FromBody] AdminRegistrationModel adminRegistrationModel)
        {
            if (adminRegistrationModel == null)
            {
                return BadRequest(new { error = "Invalid registration data." });
            }

            if (string.IsNullOrEmpty(adminRegistrationModel.Email) || string.IsNullOrEmpty(adminRegistrationModel.Password) ||
                string.IsNullOrEmpty(adminRegistrationModel.FullName) || string.IsNullOrEmpty(adminRegistrationModel.MedicalLicenseNumber) ||
                string.IsNullOrEmpty(adminRegistrationModel.Specialisation))
            {
                return BadRequest(new { error = "Email, password, full name, Medical License number and Specialisation name are required." });
            }

            string message = await serviceManager.AuthenticationService.AdminSignupService(adminRegistrationModel);

            return Ok(new { message });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> SignInUser([FromBody] LoginModel loginModel, IOptions<AppSettings> appSettings)
        {
            if (loginModel == null)
            {
                return BadRequest(new { error = "Invalid Email or Password." });
            }

            if (string.IsNullOrEmpty(loginModel.Email) || string.IsNullOrEmpty(loginModel.Password))
            {
                return BadRequest(new { error = "Email and password are required." });
            }

            var result = await serviceManager.AuthenticationService.SignInUserService(loginModel, appSettings);

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> CheckEmail([FromBody] CheckEmailRequest request)
        {
            if (string.IsNullOrEmpty(request?.Email))
            {
                return BadRequest(new { message = "Email is required!" });
            }

            bool emailExists = await serviceManager.AuthenticationService.CheckEmailService(request);

            return Ok(new { isRegistered = emailExists } );
        }
    
    }
}
