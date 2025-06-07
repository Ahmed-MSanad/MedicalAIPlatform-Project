using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.DoctorDtos
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
        public ICollection<string> DoctorPhones { get; set; } = new List<string>();
        [PersonalData]
        public string IdentificationNumber { get; set; }
        [PersonalData]
        public string MedicalLicenseNumber { get; set; }
        [PersonalData]
        public string Specialisation { get; set; }
        [PersonalData]
        public string WorkPlace { get; set; }
        [PersonalData]
        [Range(0, double.MaxValue)]
        public decimal Fee { get; set; }
        [PersonalData]
        [Range(1, 5)]
        [DisplayFormat(DataFormatString = "{0:F1}")]
        public decimal? Rate { get; set; }
        public string? Image { get; set; }
    }
}
