namespace AiDrivenMedicalPlatformAPIs.Models.Dto
{
    public class DoctorFilterDto
    {
        public string? Name { get; set; }
        public string? Speciality { get; set; }
        public decimal? MinRate { get; set; }
        public decimal? Cost { get; set; }
        public string? Workplace { get; set; }
    }
}
