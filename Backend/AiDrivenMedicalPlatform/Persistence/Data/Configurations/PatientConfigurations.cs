using MedicalProj.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class PatientConfigurations : IEntityTypeConfiguration<Patient>
    {
        public void Configure(EntityTypeBuilder<Patient> builder)
        {
            builder.HasMany(p => p.PatientPhones)
                    .WithOne(pp => pp.Patient)
                    .HasForeignKey(pp => pp.PatientId);

            builder.HasMany(p => p.Appointments)
                    .WithOne(a => a.Patient)
                    .HasForeignKey(a => a.Pid);

            builder.HasMany(patient => patient.Feedbacks)
                   .WithOne(feedback => feedback.Patient)
                   .HasForeignKey(feedback => feedback.PatientId);
        }
    }
}
