using MedicalProj.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Data.Configurations
{
    public class MedicalImageConfigurations : IEntityTypeConfiguration<MedicalImage>
    {
        public void Configure(EntityTypeBuilder<MedicalImage> builder)
        {
            builder.HasOne(medicalImage => medicalImage.Patient)
                   .WithMany(patient => patient.MedicalImages)
                   .HasForeignKey(medicalImage => medicalImage.Pid);

            builder.HasOne(medicalImage => medicalImage.Doctor)
                   .WithMany(doctor => doctor.MedicalImages)
                   .HasForeignKey(medicalImage => medicalImage.Did)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(medicalImage => medicalImage.AiAnalysis)
                   .WithOne(aiAnalysis => aiAnalysis.MedicalImage)
                   .HasForeignKey<AiAnalysis>(aiAnalysis => aiAnalysis.MedicalImageId);
        }
    }
}
