using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiDrivenMedicalPlatformAPIs.Models.Dto
{
    public class DoctorDto
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
        public ICollection<DoctorPhones> DoctorPhones { get; set; } = new List<DoctorPhones>();
        [PersonalData]
        public string IdentificationNumber { get; set; }
        [PersonalData]
        public string MedicalLicenseNumber { get; set; }
        [PersonalData]
        public string Specialisation { get; set; }
        [PersonalData]
        public string WorkPlace { get; set; }
        public byte[]? Image { get; set; }
    }
}
