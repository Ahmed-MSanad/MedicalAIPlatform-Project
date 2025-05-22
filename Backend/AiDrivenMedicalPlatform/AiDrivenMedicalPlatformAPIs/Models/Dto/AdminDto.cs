using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiDrivenMedicalPlatformAPIs.Models.Dto
{
    public class AdminDto
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
        public ICollection<string> AdminPhones { get; set; } = new List<string>();
        [PersonalData]
        public string IdentificationNumber { get; set; }
        [PersonalData]
        public string MedicalLicenseNumber { get; set; }
        [PersonalData]
        public string Specialisation { get; set; }
        public string? Image { get; set; }
    }
}
