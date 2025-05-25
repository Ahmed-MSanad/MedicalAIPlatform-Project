namespace AiDrivenMedicalPlatformAPIs.Models.Dto
{
    public class CreatedAppointmentDto
    {
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Did { get; set; }
    }
}
