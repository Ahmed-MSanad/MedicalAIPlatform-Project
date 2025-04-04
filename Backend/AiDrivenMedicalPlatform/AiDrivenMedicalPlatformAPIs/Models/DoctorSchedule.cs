using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AiDrivenMedicalPlatformAPIs.Models
{
    [PrimaryKey(nameof(DoctorId),nameof(Day))]
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
