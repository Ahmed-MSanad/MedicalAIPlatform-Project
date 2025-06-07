using Shared;

namespace MedicalProj.Data.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public string From { get; set; } = null!;
        public string To { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public NotificationType Type { get; set; }
        public DateTime SubmittedAt { get; set; }

        public string PatientId { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
    }
}