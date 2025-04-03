using AiDrivenMedicalPlatformAPIs.Types;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiDrivenMedicalPlatformAPIs.Models.Dto
{
    public class AppUserDto
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
        public string Phone { get; set; }
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
        public byte[]? Image {  get; set; }
    }
}
