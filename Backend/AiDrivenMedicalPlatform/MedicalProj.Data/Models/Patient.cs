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
        public ICollection<PatientPhone> PatientPhones { get; set; } = new List<PatientPhone>(); // Patient(1:Optional) - PatientPhones(0..N:Mandatory)
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>(); // Patient(1:Optional) - Appointment(0..N:Mandatory)
        public ICollection<MedicalImage>? MedicalImages { get; set; } = new List<MedicalImage>(); // Patient(1:Optional) - MedicalImage(0..N:Mandatory)
        public ICollection<Feedback>? Feedbacks { get; set; } = new List<Feedback>();
        public ICollection<Notification>? Notifications { get; set; }
    }
}
