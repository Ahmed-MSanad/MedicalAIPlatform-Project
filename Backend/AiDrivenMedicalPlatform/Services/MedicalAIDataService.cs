using AutoMapper;
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;
using Services.Abstraction;
using Services.Specifications;
using Shared.AiAnalysisDtos;
using Shared.PatientDtos;

namespace Services
{
    public class MedicalAIDataService(IUnitOfWork unitOfWork, IMapper mapper) : IMedicalAIDataService
    {
        public async Task<string> SetMedicalImageAiAnalysisService(AiAnalysisDto aiAnalysisDto)
        {
            int aiAnalysisId = await IsAiAnalysisNotExist(aiAnalysisDto.MedicalImageId, aiAnalysisDto.Diagnosis);
            if (aiAnalysisId == -1)
            {
                var mappedAiAnalysis = mapper.Map<AiAnalysis>(aiAnalysisDto);

                await unitOfWork.GetRepository<AiAnalysis, int>().AddAsync(mappedAiAnalysis);
            }
            else
            {
                var aiAnalysis = await unitOfWork.GetRepository<AiAnalysis, int>().GetByIdAsync(aiAnalysisId);

                mapper.Map(aiAnalysisDto, aiAnalysis);
            }
            await unitOfWork.SaveChangesAsync();

            var medicalImage = await unitOfWork.GetRepository<MedicalImage, int>().GetByIdAsync(aiAnalysisDto.MedicalImageId);

            return medicalImage!.Pid;
        }

        public async Task<IEnumerable<AiAnalysisDto>> GetMedicalImageAiAnalysisService(int medicalImageId)
        {
            var specification = new AiAnalysisWithFilterSpecification(medicalImageId);

            var aiAnalysis = await unitOfWork.GetRepository<AiAnalysis, int>().GetAllAsync(specification);
            return aiAnalysis == null
                ? throw new Exception("AI Analysis not found for the given medical image ID in Get Medical Image AI Analysis Service")
                : mapper.Map<IEnumerable<AiAnalysisDto>>(aiAnalysis);
        }

        private async Task<int> IsAiAnalysisNotExist(int medicalImageId, string Diagnosis)
        {
            var specification = new AiAnalysisWithFilterSpecification(medicalImageId);

            var aiAnalysisForTheImage = await unitOfWork.GetRepository<AiAnalysis, int>().GetAllAsync(specification);

            int aiAnalysisId = -1;
            foreach (var aiAnalysis in aiAnalysisForTheImage)
            {
                if (aiAnalysis.MedicalImageId == medicalImageId && aiAnalysis.Diagnosis == Diagnosis)
                {
                    aiAnalysisId = aiAnalysis.AiAnalysisId;
                }
            }
            return aiAnalysisId;
        }

        public async Task<PatientDto> GetMedicalImageOwnerService(int medicalImageId)
        {
            var medicalImage = await unitOfWork.GetRepository<MedicalImage, int>().GetByIdAsync(medicalImageId);

            var specification = new PatientWithFilterSpecification(medicalImage!.Pid);

            var patient = await unitOfWork.GetRepository<Patient, string>().GetByIdAsync(specification);

            return patient == null
                ? throw new Exception($"Patient assoiciated to this medical image id {medicalImageId} is not found")
                : mapper.Map<PatientDto>(patient);
        }
    }
}
