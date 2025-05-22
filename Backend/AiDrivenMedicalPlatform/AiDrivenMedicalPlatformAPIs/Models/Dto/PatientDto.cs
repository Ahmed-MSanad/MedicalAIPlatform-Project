using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiDrivenMedicalPlatformAPIs.Models.Dto
{
    public class PatientDto
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(150)")]
        public string FullName { get; set; }
        [PersonalData]
        public DateOnly DateOfBirth { get; set; }
        [PersonalData]
        public int Gender { get; set; }
        [PersonalData]
        public string Address { get; set; }
        [PersonalData]
        public ICollection<string> PatientPhones { get; set; } = new List<string>();
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
        public string? Image {  get; set; }
    }
}
