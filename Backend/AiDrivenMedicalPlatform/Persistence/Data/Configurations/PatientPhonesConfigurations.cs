using MedicalProj.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class PatientPhonesConfigurations : IEntityTypeConfiguration<PatientPhone>
    {
        public void Configure(EntityTypeBuilder<PatientPhone> builder)
        {
            builder.HasKey(pp => new { pp.Phone, pp.PatientId });

            builder.HasOne(pp => pp.Patient)
                   .WithMany(p => p.PatientPhones)
                   .HasForeignKey(pp => pp.PatientId);
        }
    }
}
