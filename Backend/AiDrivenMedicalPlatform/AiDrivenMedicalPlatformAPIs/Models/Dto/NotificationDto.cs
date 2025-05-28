using MedicalProj.Data.Models;

namespace AiDrivenMedicalPlatformAPIs.Models.Dto
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public NotificationType Type { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
