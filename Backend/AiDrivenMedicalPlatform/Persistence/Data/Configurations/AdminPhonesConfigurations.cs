
using MedicalProj.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class AdminPhonesConfigurations : IEntityTypeConfiguration<AdminPhone>
    {
        public void Configure(EntityTypeBuilder<AdminPhone> builder)
        {
            builder.HasKey(ap => new { ap.Phone, ap.AdminId });

            builder.HasOne(a => a.Admin)
                   .WithMany(ap => ap.AdminPhones)
                   .HasForeignKey(a => a.AdminId);
        }
    }
}
