using AiDrivenMedicalPlatformAPIs.Types;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AiDrivenMedicalPlatformAPIs.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }
        [PersonalData]
        public DateOnly DateOfBirth { get; set; }
        [PersonalData]
        public Gender Gender { get; set; }
        [PersonalData]
        public string Address { get; set; }
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
        [PersonalData]
        public int? HospitalId { get; set; }
    }
}
