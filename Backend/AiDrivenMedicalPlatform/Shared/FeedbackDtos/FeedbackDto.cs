namespace Shared.FeedbackDtos
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; }
        public string Message { get; set; } = null!;
        public int Rating { get; set; }
        public DateTime SubmittedAt { get; set; }
        public DateTime? RespondedAt { get; set; }
        public string? ResponseMessage { get; set; }

        public string PatientId { get; set; } = null!;
        public string? AdminId { get; set; } = null!;
    }
}
