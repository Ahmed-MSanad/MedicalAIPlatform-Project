using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using shared.AdminDtos;
using Shared.DoctorDtos;
using Shared.PatientDtos;

namespace Presentation.Controllers
{
    public class ProfileController(IServiceManager serviceManager) : ApiController
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<PatientDto>> GetPatientProfile()
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;

            var userDetails = await serviceManager.ProfileService.GetPatientProfileService(userId);

            return Ok(userDetails);
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<DoctorDto>> GetDoctorProfile()
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;

            var doctorDto = await serviceManager.ProfileService.GetDoctorProfileService(userId);

            return Ok(doctorDto);
        }


        [Authorize]
        [HttpGet]
        public async Task<ActionResult<AdminDto>> GetAdminProfile()
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;

            var adminDto = await serviceManager.ProfileService.GetAdminProfileService(userId);

            return Ok(adminDto);
        }


        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteUserProfile()
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;

            await serviceManager.ProfileService.DeleteUserProfileService(userId);

            return Ok(new { message = "User Deleted Successfully" });
        }


        [Authorize]
        [HttpPut]
        public async Task<ActionResult> EditPatientProfile([FromBody] PatientDto updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserID")?.Value;
            if (userId == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized ,new { Message= "User ID claim not found in token." });
            }

            await serviceManager.ProfileService.EditPatientProfileService(updateRequest, userId);

            return Ok(new { Message = "Patient Profile updated successfully" });
        }


        [Authorize]
        [HttpPut]
        public async Task<ActionResult> EditDoctorProfile([FromBody] DoctorDto updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserID")?.Value;
            if (userId == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { Message = "User ID claim not found in token." });
            }

            await serviceManager.ProfileService.EditDoctorProfileService(updateRequest, userId);

            return Ok(new { Message = "Doctor Profile updated successfully" });
        }


        [Authorize]
        [HttpPut]
        public async Task<ActionResult> EditAdminProfile([FromBody] AdminDto updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserID")?.Value;
            if (userId == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized ,new { Message = "User ID claim not found in token." });
            }

            await serviceManager.ProfileService.EditAdminProfileService(updateRequest, userId);

            return Ok(new { Message = "Admin Profile updated successfully" });
        }
    
    }
}
