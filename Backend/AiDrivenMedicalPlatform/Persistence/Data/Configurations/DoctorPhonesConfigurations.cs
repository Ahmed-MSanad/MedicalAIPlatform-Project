using MedicalProj.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class DoctorPhonesConfigurations : IEntityTypeConfiguration<DoctorPhone>
    {
        public void Configure(EntityTypeBuilder<DoctorPhone> builder)
        {
            builder.HasKey(dp => new { dp.Phone, dp.DoctorId });

            builder.HasOne(dp => dp.Doctor)
                   .WithMany(d => d.DoctorPhones)
                   .HasForeignKey(dp => dp.DoctorId);
        }
    }
}
