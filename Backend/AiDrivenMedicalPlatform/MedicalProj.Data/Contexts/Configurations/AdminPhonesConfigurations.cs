
using MedicalProj.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalProj.Data.Contexts.Configurations
{
    public class AdminPhonesConfigurations : IEntityTypeConfiguration<AdminPhones>
    {
        public void Configure(EntityTypeBuilder<AdminPhones> builder)
        {
            builder.HasKey(ap => new { ap.Phone, ap.AdminId });

            builder.HasOne(a => a.Admin)
                   .WithMany(ap => ap.AdminPhones)
                   .HasForeignKey(a => a.AdminId);
        }
    }
}
