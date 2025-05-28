using MedicalProj.Data.Contexts;
using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using AiDrivenMedicalPlatformAPIs.Models.Dto;

namespace AiDrivenMedicalPlatformAPIs.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly MedicalDbContext dbContext;
        private readonly SmtpSettings _smtpSettings;

        public NotificationController(MedicalDbContext _dbContext, IOptions<SmtpSettings> smtpSettings)
        {
            dbContext = _dbContext;
            _smtpSettings = smtpSettings.Value;
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

                var patient = await dbContext.Patients.FirstOrDefaultAsync(p => p.Id == patientId);
                if (patient == null)
                {
                    return NotFound("Patient not found");
                }

                string subject = "", message = "";
                switch (NotificationType)
                {
                    case NotificationType.Alert:
                        subject = "Alert Notification";
                        message = "This is an alert message. Please take action immediately.";
                        break;
                    case NotificationType.Reminder:
                        subject = "Reminder Notification";
                        message = "This is a reminder for your upcoming Appoinment.";
                        break;
                    case NotificationType.Success:
                        subject = "Success Notification";
                        message = "Your Appoinment has been reserved successfully!";
                        break;
                    default:
                        throw new ArgumentException("Invalid notification type. Use 'alert', 'reminder', or 'success'.");
                }


                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("MedicalAiPlatform", _smtpSettings.Username));
                email.To.Add(new MailboxAddress("", patient.Email!));
                email.Subject = subject;

                var bodyBuilder = new BodyBuilder { TextBody = message };
                email.Body = bodyBuilder.ToMessageBody();

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);

                var notification = new Notification
                {
                    Message = message,
                    From = _smtpSettings.Username,
                    To = patient.Email!,
                    Subject = subject,
                    Type = NotificationType,
                    SubmittedAt = DateTime.Now,
                    PatientId = patientId
                };

                await dbContext.Notifications.AddAsync(notification);
                await dbContext.SaveChangesAsync();

                return Ok(new { message = "Email sent successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Failed to send email: {ex.Message}" });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> getNotifications()
        {
            try
            {
                var patientId = User.Claims.FirstOrDefault(u => u.Type == "UserID")?.Value;
                if (patientId is null)
                {
                    return BadRequest("User ID is required");
                }

                var notifications = await dbContext.Notifications
                                                   .Where(n => n.PatientId == patientId)
                                                   .OrderByDescending(n => n.SubmittedAt)
                                                   .Select(n => new NotificationDto
                                                   {
                                                       Id = n.Id,
                                                       Message = n.Message,
                                                       From = n.From,
                                                       To = n.To,
                                                       Subject = n.Subject,
                                                       Type = n.Type,
                                                       SubmittedAt = n.SubmittedAt
                                                   })
                                                   .ToListAsync();

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
                var notification = await dbContext.Notifications.Where(n => n.Id == NotificationId).FirstOrDefaultAsync();

                if(notification is null)
                {
                    return NotFound(new { error = $"Notification of id {notification} is not found" });
                }

                dbContext.Remove(notification);

                dbContext.SaveChanges();

                return Ok(new { message = $"The \"{notification.Subject}\" Notification is Deleted Successfully" });

            }catch(Exception ex)
            {
                return StatusCode(500, new { error = $"Failed to remove notification of id {NotificationId}: {ex.Message}" });
            }
        }
    }

    public class SmtpSettings
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
