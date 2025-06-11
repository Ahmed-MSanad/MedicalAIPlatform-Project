using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.MedicalImageDtos;

namespace Services.Abstraction
{
    public interface IMedicalImageService
    {
        public Task AddMedicalImageService(CreatedMedicalImageDto medicalImageDto, string patientId);
        public Task EditMedicalImageService(int MedicalImageId, byte[] newImage);
        public Task DeleteMedicalImageService(int MedicalImageId);
        public Task<MedicalImageDto> GetMedicalImageService(int appointmentId);

    }
}
