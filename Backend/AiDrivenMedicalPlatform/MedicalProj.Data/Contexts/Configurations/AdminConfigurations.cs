using System.Reflection.Emit;
using MedicalProj.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalProj.Data.Contexts.Configurations
{
    public class AdminConfigurations : IEntityTypeConfiguration<Admin>
    {
        public void Configure(EntityTypeBuilder<Admin> builder)
        {
            builder.HasMany(a => a.AdminPhones)
                    .WithOne(ap => ap.Admin)
                    .HasForeignKey(ap => ap.AdminId);
        }
    }
}
