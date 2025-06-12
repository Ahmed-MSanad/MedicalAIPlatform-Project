using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction;
using Services.Specifications;
using Shared.MedicalImageDtos;

namespace Services
{
    public class MedicalImageService(IUnitOfWork unitOfWork, IMapper mapper) : IMedicalImageService
    {
        public async Task AddMedicalImageService(CreatedMedicalImageDto medicalImageDto, string patientId)
        {
            var medicalImage = mapper.Map<MedicalImage>(medicalImageDto);

            medicalImage.Pid = patientId;

            await unitOfWork.GetRepository<MedicalImage, int>().AddAsync(medicalImage);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMedicalImageService(int MedicalImageId)
        {
            var medicalImage = await unitOfWork.GetRepository<MedicalImage,int>().GetByIdAsync(MedicalImageId);

            if (medicalImage is null)
            {
                throw new Exception("Image Not Found");
            }

            unitOfWork.GetRepository<MedicalImage, int>().Delete(medicalImage);

            await unitOfWork.SaveChangesAsync();
        }

        public async Task EditMedicalImageService(int medicalImageId, NewImageDto newImageDto)
        {
            var medicalImage = await unitOfWork.GetRepository<MedicalImage, int>().GetByIdAsync(medicalImageId);

            if (medicalImage is null)
            {
                throw new Exception("Image Not Found");
            }

            var specifications = new AiAnalysisWithFilterSpecification(medicalImageId);

            var aiAnalysis = await unitOfWork.GetRepository<AiAnalysis, int>().GetByIdAsync(specifications);

            if(aiAnalysis is not null)
            {
                unitOfWork.GetRepository<AiAnalysis,int>().Delete(aiAnalysis);
            }

            medicalImage.Image = newImageDto.Image;

            medicalImage.UploadDate = DateTime.Now;

            unitOfWork.GetRepository<MedicalImage, int>().Update(medicalImage);

            await unitOfWork.SaveChangesAsync();

        }

        public async Task<MedicalImageDto> GetMedicalImageService(int appointmentId)
        {
            var specifications = new MedicalImageWithFilterSpecification(appointmentId);

            var medicalImage = await unitOfWork.GetRepository<MedicalImage,int>().GetByIdAsync(specifications);

            if (medicalImage is null)
            {
                throw new Exception("Image Not Found");
            }

            var mappedImage = mapper.Map<MedicalImageDto>(medicalImage);

            return mappedImage;
        }
    }
}
