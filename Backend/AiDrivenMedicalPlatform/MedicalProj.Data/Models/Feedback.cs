namespace MedicalProj.Data.Models
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string Message { get; set; } = null!;
        public int Rating { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime RespondedAt { get; set; }
        public string ResponseMessage { get; set; } = null!;

        public string PatientId { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public string AdminId { get; set; } = null!;
        public Admin Admin { get; set; } = null!;
    }
}
