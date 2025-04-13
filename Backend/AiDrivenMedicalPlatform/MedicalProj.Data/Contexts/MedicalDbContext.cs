using System.Reflection;
using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MedicalProj.Data.Contexts
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
        public DbSet<PatientPhones> PatientPhones { get; set; }
        public DbSet<DoctorPhones> DoctorPhones { get; set; }
        public DbSet<AdminPhones> AdminPhones { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
    }
}
