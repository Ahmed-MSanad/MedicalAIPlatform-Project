using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction;
using Shared.FeedbackDtos;

namespace Presentation.Controllers
{
    public class FeedbackController : ApiController
    {
        private readonly IServiceManager serviceManager;

        public FeedbackController(IServiceManager _serviceManager)
        {
            serviceManager = _serviceManager;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<FeedbackDto>> patientFeedback([FromBody] FeedbackMessageDto feedbackDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var patientId = User.Claims.FirstOrDefault(u => u.Type == "UserID");
                if (patientId == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { error = "Patient ID claim not found in token." });
                }

                var newFeedback = await serviceManager.FeedbackService.SendPatientFeedbackAsync(feedbackDto, patientId.Value);

                return Ok(newFeedback);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Failed to create user feedback: {ex.Message}" });
            }
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut]
        public async Task<ActionResult<FeedbackDto>> adminResponse([FromBody] FeedbackResponseDto responseDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var adminId = User.Claims.FirstOrDefault(u => u.Type == "UserID");
                if (adminId == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { error = "Admin ID claim not found in token." });
                }

                var updatedFeedbackDto = await serviceManager.FeedbackService.SendAdminResponse(responseDto, adminId.Value);
                if (updatedFeedbackDto == null)
                {
                    return NotFound(new { error = $"Can't Found a Feedback with id: {responseDto.FeedbackId}" });
                }

                return Ok(updatedFeedbackDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Admin Response Failed: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDto>>> getPatientFeedBacks()
        {
            try
            {
                var feedbacks = await serviceManager.FeedbackService.GetAllFeedbacks();

                if (feedbacks == null || feedbacks.Count() == 0)
                {
                    return NotFound(new { error = "No feedbacks found." });
                }

                return Ok(feedbacks);
            }
            catch (Exception ex) {
                return BadRequest(new { error = $"Failed to get feedbacks: {ex.Message}" });
            }
        }


        [Authorize(Policy = "PatientsAndAdminsOnly")]
        [HttpDelete("{feedbackId}")]
        public async Task<ActionResult> removeFeedBack([FromRoute] int feedbackId) // any admin or right patient are allowed to delete the feedback.
        {
            try
            {
                var userRole = User.Claims.FirstOrDefault(u => u.Type == ClaimTypes.Role);
                if (userRole == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { error = "User Role claim not found in token." });
                }

                var userId = User.Claims.FirstOrDefault(u => u.Type == "UserID");
                if (userId == null)
                {
                    return StatusCode(StatusCodes.Status401Unauthorized, new { error = "User ID claim not found in token." });
                }

                var feedback = await serviceManager.FeedbackService.GetFeedBack(feedbackId);
                if (feedback == null)
                {
                    return NotFound(new { error = $"Feedback with ID {feedbackId} is not found." });
                }

                if (userRole.Value != "Admin" && feedback.PatientId != userId.Value)
                {
                    return StatusCode((int)HttpStatusCode.Forbidden, new { error = "You are not authorized to delete this feedback." });
                }

                await serviceManager.FeedbackService.RemoveFeedBack(feedbackId);

                return Ok(new { message = "Feedback removed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Failed to get feedbacks: {ex.Message}" });
            }
        }
    }
}
