﻿using Shared.AppointmentDtos;

namespace MedicalProj.Data.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public DateTime Date { get; set; }
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public decimal Cost { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string Did { get; set; }
        public string Pid { get; set; }
        public bool IsRated { get; set; } = false;
        // Navigation properties :
        public Doctor Doctor { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public MedicalImage? MedicalImage { get; set; }
    }
}
