using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public ICollection<AdminPhones> AdminPhones { get; set; } = new List<AdminPhones>();
    }
}
