using Shared.AiAnalysisDtos;
using Shared.PatientDtos;

namespace Services.Abstraction
{
    public interface IMedicalAIDataService
    {
        public Task<string> SetMedicalImageAiAnalysisService(AiAnalysisDto aiAnalysisDto);
        public Task<IEnumerable<AiAnalysisDto>> GetMedicalImageAiAnalysisService(int medicalImageId);
        public Task<PatientDto> GetMedicalImageOwnerService(int medicalImageId);
    }
}
