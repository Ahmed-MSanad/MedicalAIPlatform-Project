using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.AiAnalysisDtos
{
    public class AiAnalysisDto
    {
        public decimal ConfidenceScore { get; set; }
        public string ExplanationDetails { get; set; } = null!;
        public string Diagnosis { get; set; } = null!;
        public string? image { get; set; } = null!;
        public int MedicalImageId { get; set; }
    }
}
