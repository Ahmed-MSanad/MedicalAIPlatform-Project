using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using shared.AdminDtos;
using Shared.DoctorDtos;
using Shared.PatientDtos;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public class ProfileController(IServiceManager serviceManager) : ApiController
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetProfile()
        {
            string userId = User.Claims.First(x => x.Type == "UserID").Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
        
            if (role == "Patient")
                return Ok(await serviceManager.ProfileService.GetPatientProfileService(userId));

            if (role == "Doctor")
                return Ok(await serviceManager.ProfileService.GetDoctorProfileService(userId));

            if (role == "Admin")
                return Ok(await serviceManager.ProfileService.GetAdminProfileService(userId));

            return BadRequest("Invalid role");
        }

        [Authorize]
        [HttpDelete]
        public async Task<ActionResult> DeleteUserProfile()
        {
            try{
            string userId = User.Claims.First(x => x.Type == "UserID").Value;

            await serviceManager.ProfileService.DeleteUserProfileService(userId);

            return Ok(new { message = "User Deleted Successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }


        [Authorize]
        [HttpPut("patient")]
        public async Task<ActionResult> EditPatientProfile([FromBody] PatientDto updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserID")?.Value;
            if (userId == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User ID claim not found in token." });
            }

            await serviceManager.ProfileService.EditPatientProfileService(updateRequest, userId);

            return Ok(new { message = "Patient Profile updated successfully" });
        }


        [Authorize]
        [HttpPut("doctor")]
        public async Task<ActionResult> EditDoctorProfile([FromBody] DoctorDto updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserID")?.Value;
            if (userId == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User ID claim not found in token." });
            }

            await serviceManager.ProfileService.EditDoctorProfileService(updateRequest, userId);

            return Ok(new { message = "Doctor Profile updated successfully" });
        }


        [Authorize]
        [HttpPut("admin")]
        public async Task<ActionResult> EditAdminProfile([FromBody] AdminDto updateRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = User.Claims.FirstOrDefault(x => x.Type == "UserID")?.Value;
            if (userId == null)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = "User ID claim not found in token." });
            }

            await serviceManager.ProfileService.EditAdminProfileService(updateRequest, userId);

            return Ok(new { message = "Admin Profile updated successfully" });
        }

    }
}
