
namespace Shared.AiAnalysisDtos
{
    public class AiAnalysisDto
    {
        public decimal ConfidenceScore { get; set; }
        public string DiseaseType { get; set; }
        public string ExplanationDetails { get; set; } = null!;
        public string Diagnosis { get; set; } = null!;
        public string? image { get; set; } = null!;
        public int MedicalImageId { get; set; }
    }
}
