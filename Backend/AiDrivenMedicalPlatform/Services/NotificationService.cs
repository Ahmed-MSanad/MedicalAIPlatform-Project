using AutoMapper;
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;
using Services.Abstraction;
using Shared;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using Services.Specifications;
using Shared.NotificationDtos;

namespace Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly SmtpSettings smtpSettings;

        public NotificationService(IUnitOfWork _unitOfWork, IMapper _mapper, IOptions<SmtpSettings> _smtpSettings)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
            smtpSettings = _smtpSettings.Value;
        }

        public async Task SendEmailToPatient(NotificationType NotificationType, string patientId)
        {
            var patient = await unitOfWork.GetRepository<Patient, string>().GetByIdAsync(patientId);
            if (patient == null)
            {
                throw new KeyNotFoundException($"Patient with ID {patientId} not found.");
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
            email.From.Add(new MailboxAddress("MedicalAiPlatform", smtpSettings.Username));
            email.To.Add(new MailboxAddress("", patient.Email!));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder { TextBody = message };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(smtpSettings.Host, smtpSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(smtpSettings.Username, smtpSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            var notification = new Notification
            {
                Message = message,
                From = smtpSettings.Username,
                To = patient.Email!,
                Subject = subject,
                Type = NotificationType,
                SubmittedAt = DateTime.Now,
                PatientId = patientId
            };

            await unitOfWork.GetRepository<Notification, int>().AddAsync(notification);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<NotificationDto>> GetPatientNotifications(string patientId)
        {
            var specification = new NotificationWithFilterSpecification(patientId);
            
            var notifications = await unitOfWork.GetRepository<Notification, int>().GetAllAsync(specification);

            var notificationsDto = mapper.Map<IEnumerable<NotificationDto>>(notifications);

            return notificationsDto;
        }

        public async Task RemovePatientNotification(int NotificationId)
        {
            var notification = await unitOfWork.GetRepository<Notification, int>().GetByIdAsync(NotificationId);

            if (notification is null)
            {
                throw new Exception($"Notification to be deleted of id {notification} is not found");
            }

            unitOfWork.GetRepository<Notification, int>().Delete(notification);

            await unitOfWork.SaveChangesAsync();
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
