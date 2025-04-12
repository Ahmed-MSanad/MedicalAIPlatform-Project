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
        public ICollection<DoctorPhones> DoctorPhones { get; set; } = new List<DoctorPhones>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
