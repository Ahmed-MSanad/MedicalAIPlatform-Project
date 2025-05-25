using System.Net;
using System.Security.Claims;
using AiDrivenMedicalPlatformAPIs.Models.Dto;
using MedicalProj.Data.Contexts;
using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly MedicalDbContext dbContext;

        public FeedbackController(MedicalDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> patientFeedback([FromBody] FeedbackMessageDto feedbackDto)
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
                    return Unauthorized(new { error = "Patient ID claim not found in token." });
                }

                var newFeedback = new Feedback()
                {
                    PatientId = patientId.Value,
                    Message = feedbackDto.Message,
                    Rating = feedbackDto.Rating,
                    SubmittedAt = DateTime.Now,
                };
                await dbContext.Feedbacks.AddAsync(newFeedback);
                await dbContext.SaveChangesAsync();

                return Ok(newFeedback);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Failed to create user feedback: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<ActionResult> adminResponse([FromBody] FeedbackResponseDto responseDto)
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
                    return Unauthorized(new { error = "Admin ID claim not found in token." });
                }

                var oldFeedback = await dbContext.Feedbacks.FirstOrDefaultAsync(f => f.FeedbackId == responseDto.FeedbackId);
                if (oldFeedback == null)
                {
                    return NotFound(new { error = $"Can't Found a Feedback with id: {responseDto.FeedbackId}" });
                }

                oldFeedback.ResponseMessage = responseDto.ResponseMessage;
                oldFeedback.AdminId = adminId.Value;
                oldFeedback.RespondedAt = DateTime.Now;

                await dbContext.SaveChangesAsync();

                return Ok(oldFeedback);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Admin Response Failed: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> getPatientFeedBacks()
        {
            try
            {
                var feedbacks = await dbContext.Feedbacks.Select(f => new FeedbackDto
                {
                    FeedbackId = f.FeedbackId,
                    Message = f.Message,
                    Rating = f.Rating,
                    SubmittedAt = f.SubmittedAt,
                    RespondedAt = f.RespondedAt,
                    ResponseMessage = f.ResponseMessage,
                    PatientId = f.PatientId,
                    AdminId = f.AdminId,
                }).ToListAsync();

                if (feedbacks == null || feedbacks.Count == 0)
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
                    return Unauthorized(new { error = "User Role claim not found in token." });
                }

                var userId = User.Claims.FirstOrDefault(u => u.Type == "UserID");
                if (userId == null)
                {
                    return Unauthorized(new { error = "User ID claim not found in token." });
                }

                var feedback = await dbContext.Feedbacks.FindAsync(feedbackId);
                if(feedback == null)
                {
                    return NotFound(new { error = $"Feedback with ID {feedbackId} is not found." });
                }

                if (userRole.Value != "Admin" && feedback.PatientId != userId.Value)
                {
                    return StatusCode((int)HttpStatusCode.Forbidden, new { error = "You are not authorized to delete this feedback." });
                }

                dbContext.Feedbacks.Remove(feedback);
                
                await dbContext.SaveChangesAsync();

                return Ok(new { message = "Feedback removed successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = $"Failed to get feedbacks: {ex.Message}" });
            }
        }
    }
}
