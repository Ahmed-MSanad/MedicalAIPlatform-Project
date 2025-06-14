﻿using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MedicalProj.Data.Models
{
    public class Doctor : AppUser
    {
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
        public int TotalRating { get; set; } = 0;
        public int NumberOfRaters { get; set; } = 0;

        // Navigation properties :
        public ICollection<DoctorPhone> DoctorPhones { get; set; } = new List<DoctorPhone>(); // Doctor(1:Optional) - DoctorPhone(0..N:Mandatory)
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>(); // Doctor(1:Optional) - Appointment(0..N:Mandatory)
        public ICollection<DoctorSchedule> DoctorSchedule { get; set; } = null!; // Doctor(1:Optionally) - DoctorSchedule(0..N:Mandatory)
        public ICollection<MedicalImage>? MedicalImages { get; set; } = new List<MedicalImage>(); // Doctor(1:Optional) - MedicalImage(0..N:Mandatory)

        public static implicit operator Doctor(ValueTask<Doctor?> v)
        {
            throw new NotImplementedException();
        }
    }
}
