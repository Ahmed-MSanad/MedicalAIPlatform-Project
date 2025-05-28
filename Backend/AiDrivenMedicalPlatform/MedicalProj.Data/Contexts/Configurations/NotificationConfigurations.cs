using MedicalProj.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalProj.Data.Contexts.Configurations
{
    public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.HasOne(n => n.Patient)
                   .WithMany(p => p.Notifications)
                   .HasForeignKey(n => n.PatientId);
        }
    }
}
