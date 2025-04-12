using Microsoft.AspNetCore.Identity;


namespace MedicalProj.Data.Models
{
    public class Patient : AppUser
    {
        [PersonalData]
        public string Occupation { get; set; }
        [PersonalData]
        public string EmergencyContactName { get; set; }
        [PersonalData]
        public string EmergencyContactNumber { get; set; }
        [PersonalData]
        public string FamilyMedicalHistory { get; set; }
        [PersonalData]
        public string PastMedicalHistory { get; set; }

        //Navigation properties :
        public ICollection<PatientPhones> PatientPhones { get; set; } = new List<PatientPhones>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
