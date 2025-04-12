using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MedicalProj.Data.Models
{
    [PrimaryKey(nameof(DoctorId), nameof(Day))]
    public class DoctorSchedule
    {
        public string DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public virtual AppUser Doctor { get; set; }
        public DayOfWeek Day { get; set; }

        [Required]
        public TimeSpan From { get; set; }
        [Required]
        public TimeSpan To { get; set; }
    }
}
