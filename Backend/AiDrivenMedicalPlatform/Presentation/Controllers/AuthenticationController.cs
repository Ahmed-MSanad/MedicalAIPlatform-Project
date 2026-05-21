using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Abstraction;
using Shared;
using Shared.AuthenticationDtos;

namespace Presentation.Controllers
{
    [Route("api/auth")]
    public class AuthenticationController(IServiceManager serviceManager) : ApiController
    {
        [AllowAnonymous]
        [HttpPost("signup/patient")]
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
        [HttpPost("signup/doctor")]
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
        [HttpPost("signup/admin")]
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
        [HttpPost("login")]
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
        [HttpPost("check-email")]
        public async Task<ActionResult> CheckEmail([FromBody] CheckEmailRequest request)
        {
            if (string.IsNullOrEmpty(request?.Email))
            {
                return BadRequest(new { message = "Email is required!" });
            }

            bool emailExists = await serviceManager.AuthenticationService.CheckEmailService(request);

            return Ok(new { isRegistered = emailExists } );
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<ActionResult> ForgetPassword([FromBody] CheckEmailRequest emailRequest)
        {
            bool isEmailExist = await serviceManager.AuthenticationService.CheckEmailService(emailRequest);

            if (!isEmailExist)
                return NotFound(new { error = "Email does not exist." });

            await serviceManager.AuthenticationService.ForgetPasswordService(emailRequest);

            return Ok(new { message = "Password reset link has been sent to your email address. Please check your inbox." });
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            CheckEmailRequest emailRequest = new() { Email = resetPasswordDto.Email };

            bool isEmailExist = await serviceManager.AuthenticationService.CheckEmailService(emailRequest);

            if (!isEmailExist)
                return NotFound(new { error = "Email does not exist." });

            var result = await serviceManager.AuthenticationService.ResetPasswordService(resetPasswordDto);

            if (result.Succeeded)
                return Ok(new { message = "Password reset successfully." });
            return BadRequest(new { error = result.Errors });
        }
    }
}
