using System.Reflection;
using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Data
{
    public class MedicalDbContext : IdentityDbContext<AppUser>
    {
        public MedicalDbContext(DbContextOptions<MedicalDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Configure TPT for AppUser and its derived types
            //modelBuilder.Entity<AppUser>()
            //    .ToTable("AspNetUsers");

            modelBuilder.Entity<Patient>()
                .ToTable("Patients"); // Separate table for Patient

            modelBuilder.Entity<Doctor>()
                .ToTable("Doctors"); // Separate table for Doctor

            modelBuilder.Entity<Admin>()
                .ToTable("Admins"); // Separate table for Admin

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<PatientPhone> PatientPhones { get; set; }
        public DbSet<DoctorPhone> DoctorPhones { get; set; }
        public DbSet<AdminPhone> AdminPhones { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<MedicalImage> MedicalImages { get; set; }
        public DbSet<AiAnalysis> AiAnalyses { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
