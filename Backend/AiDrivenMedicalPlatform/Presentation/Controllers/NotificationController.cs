using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared;
using Services.Abstraction;
using Shared.NotificationDtos;

namespace Presentation.Controllers
{
    public class NotificationController : ApiController
    {

        private readonly IServiceManager _serviceManager;

        public NotificationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [Authorize]
        [HttpPost("{NotificationType}")]
        public async Task<IActionResult> SendEmail([FromRoute] NotificationType NotificationType)
        {
            try
            {
                var patientId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                if (string.IsNullOrEmpty(patientId))
                {
                    return BadRequest("User ID is required");
                }

                await _serviceManager.NotificationService.SendEmailToPatient(NotificationType, patientId);

                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Failed to send email: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> getNotifications()
        {
            try
            {
                var patientId = User.Claims.FirstOrDefault(u => u.Type == "UserID")?.Value;
                if (patientId is null)
                {
                    return BadRequest("User ID is required");
                }

                var notifications = await _serviceManager.NotificationService.GetPatientNotifications(patientId);

                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Failed to retrieve notifications: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpDelete("{NotificationId}")]
        public async Task<ActionResult> removeNotification([FromRoute] int NotificationId)
        {
            try
            {
                await _serviceManager.NotificationService.RemovePatientNotification(NotificationId);

                return Ok(new { message = $"The Notification is Deleted Successfully" });

            }catch(Exception ex)
            {
                return StatusCode(500, new { error = $"Failed to remove notification of id {NotificationId}: {ex.Message}" });
            }
        }
    }
}
