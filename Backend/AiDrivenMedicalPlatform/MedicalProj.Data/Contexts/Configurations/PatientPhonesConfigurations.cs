using MedicalProj.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalProj.Data.Contexts.Configurations
{
    public class PatientPhonesConfigurations : IEntityTypeConfiguration<PatientPhones>
    {
        public void Configure(EntityTypeBuilder<PatientPhones> builder)
        {
            builder.HasKey(pp => new { pp.Phone, pp.PatientId });

            builder.HasOne(pp => pp.Patient)
                   .WithMany(p => p.PatientPhones)
                   .HasForeignKey(pp => pp.PatientId);
        }
    }
}
