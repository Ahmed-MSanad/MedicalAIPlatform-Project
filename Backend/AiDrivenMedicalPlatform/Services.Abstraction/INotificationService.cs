using Shared;
using Shared.NotificationDtos;

namespace Services.Abstraction
{
    public interface INotificationService
    {
        public Task SendEmailToPatient(NotificationType NotificationType, string patientId);
        public Task<IEnumerable<NotificationDto>> GetPatientNotifications(string patientId);
        public Task RemovePatientNotification(int NotificationId);
    }
}
