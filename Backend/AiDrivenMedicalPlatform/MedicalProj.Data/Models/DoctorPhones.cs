using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalProj.Data.Models
{
    public class DoctorPhones
    {
        public string Phone { get; set; } = string.Empty;
        public string DoctorId { get; set; }

        // EF Core will create a foreign key for this property -> Navigation properties :
        public Doctor Doctor { get; set; } = null!;
    }
}
