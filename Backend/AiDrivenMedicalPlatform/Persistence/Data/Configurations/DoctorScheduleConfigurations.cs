using MedicalProj.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class DoctorScheduleConfigurations : IEntityTypeConfiguration<DoctorSchedule>
    {
        public void Configure(EntityTypeBuilder<DoctorSchedule> builder)
        {
            builder.HasKey(ds => new { ds.DoctorId, ds.Day });

            builder.HasOne(ds => ds.Doctor)
                   .WithMany(d => d.DoctorSchedule)
                   .HasForeignKey(ds => ds.DoctorId);

            builder.HasIndex(d => d.DoctorId)
                   .IsUnique(false);
        }
    }
}
