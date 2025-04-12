using MedicalProj.Data.Types;

namespace MedicalProj.Data.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public AppointmentStatus Status { get; set; }
        public decimal Cost { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string Did { get; set; }
        public string Pid { get; set; }

        // Navigation properties :
        public Doctor Doctor { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
    }
}
