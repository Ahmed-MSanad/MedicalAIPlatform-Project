using Microsoft.AspNetCore.Identity;

namespace MedicalProj.Data.Models
{
    public class Admin : AppUser
    {
        [PersonalData]
        public string IdentificationNumber { get; set; }
        [PersonalData]
        public string MedicalLicenseNumber { get; set; }
        [PersonalData]
        public string Specialisation { get; set; }

        //Navigation properties :
        public ICollection<AdminPhone> AdminPhones { get; set; } = new List<AdminPhone>();
        public ICollection<Feedback>? Feedbacks { get; set; } = new List<Feedback>();
    }
}
