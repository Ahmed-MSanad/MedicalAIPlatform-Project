using Microsoft.AspNetCore.Identity;

namespace AiDrivenMedicalPlatformAPIs.Models.Dto
{
    public class AppointmentInfoDto
    {
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public string Location { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public string Description { get; set; }
        
    }
}
