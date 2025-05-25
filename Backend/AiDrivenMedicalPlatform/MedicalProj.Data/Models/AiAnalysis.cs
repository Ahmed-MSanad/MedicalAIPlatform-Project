namespace MedicalProj.Data.Models
{
    public class AiAnalysis
    {
        public int AiAnalysisId { get; set; }
        public string Diagnosis { get; set; } = null!;
        public decimal ConfidenceScore { get; set; }
        public string HeatmapData { get; set; } = null!;
        public string ExplanationDetails { get; set; } = null!;
        public DateTime AnalysisDate { get; set; }

        public int MedicalImageId { get; set; }
        public MedicalImage MedicalImage { get; set; } = null!;
    }
}
