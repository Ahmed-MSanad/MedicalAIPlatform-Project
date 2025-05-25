using System.ComponentModel.DataAnnotations;

namespace MedicalProj.Data.Models
{
    public class DoctorSchedule
    {
        public string Day { get; set; }

        [Required]
        public TimeSpan From { get; set; }
        [Required]
        public TimeSpan To { get; set; }
        public string DoctorId { get; set; }

        public Doctor? Doctor { get; set; } // 1:Mandatory - 1:Mandatory
    }
}
