using MedicalProj.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedicalProj.Data.Contexts.Configurations
{
    public class FeedbackConfigurations : IEntityTypeConfiguration<Feedback>
    {
        public void Configure(EntityTypeBuilder<Feedback> builder)
        {
            builder.HasOne(feedback => feedback.Admin)
                   .WithMany(admin => admin.Feedbacks)
                   .HasForeignKey(feedback => feedback.AdminId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
