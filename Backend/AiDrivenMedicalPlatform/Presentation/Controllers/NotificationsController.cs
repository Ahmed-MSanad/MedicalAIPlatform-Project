using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared;
using Services.Abstraction;
using Shared.NotificationDtos;

namespace Presentation.Controllers
{
    [Authorize]
    public class NotificationsController : ApiController
    {

        private readonly IServiceManager _serviceManager;

        public NotificationsController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromQuery] NotificationType notificationType)
        {
            try
            {
                var patientId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
                if (string.IsNullOrEmpty(patientId))
                {
                    return BadRequest("User ID is required");
                }

                await _serviceManager.NotificationService.SendEmailToPatient(notificationType, patientId);

                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Failed to send email: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificationDto>>> GetNotifications()
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

        [HttpDelete("{notificationId}")]
        public async Task<ActionResult> RemoveNotification([FromRoute] int notificationId)
        {
            try
            {
                await _serviceManager.NotificationService.RemovePatientNotification(notificationId);

                return Ok(new { message = "The Notification is Deleted Successfully" });

            }catch(Exception ex)
            {
                return StatusCode(500, new { error = $"Failed to remove notification of id {notificationId}: {ex.Message}" });
            }
        }
    }
}
