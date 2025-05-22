using MedicalProj.Data.Types;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MedicalProj.Data.Models
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
        public byte[]? Image { get; set; }

        //[PersonalData]
        //public int? HospitalId { get; set; }
    }
}
