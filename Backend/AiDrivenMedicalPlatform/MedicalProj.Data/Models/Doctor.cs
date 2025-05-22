using Microsoft.AspNetCore.Identity;

namespace MedicalProj.Data.Models
{
    public class Doctor : AppUser
    {
        [PersonalData]
        public string IdentificationNumber { get; set; }
        [PersonalData]
        public string MedicalLicenseNumber { get; set; }
        [PersonalData]
        public string Specialisation { get; set; }
        [PersonalData]
        public string WorkPlace { get; set; }

        // Navigation properties :
        public ICollection<DoctorPhone> DoctorPhones { get; set; } = new List<DoctorPhone>(); // Doctor(1:Optional) - DoctorPhone(0..N:Mandatory)
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>(); // Doctor(1:Optional) - Appointment(0..N:Mandatory)
        public DoctorSchedule DoctorSchedule { get; set; } = null!; // Doctor(1:Mandatory) - DoctorSchedule(1:Mandatory)
        public ICollection<MedicalImage>? MedicalImages { get; set; } = new List<MedicalImage>(); // Doctor(1:Optional) - MedicalImage(0..N:Mandatory)
    }
}
