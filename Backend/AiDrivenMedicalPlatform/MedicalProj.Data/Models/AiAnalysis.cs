using System.ComponentModel.DataAnnotations;

namespace MedicalProj.Data.Models
{
    public class AiAnalysis
    {
        public int AiAnalysisId { get; set; }
        public decimal ConfidenceScore { get; set; }
        public string Diagnosis { get; set; } = null!;
        public string ExplanationDetails { get; set; } = null!;
        public byte[]? image { get; set; } = null!;
        public int MedicalImageId { get; set; }
        public MedicalImage MedicalImage { get; set; } = null!;
    }
}
