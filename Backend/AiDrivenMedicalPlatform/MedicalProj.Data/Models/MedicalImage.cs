namespace MedicalProj.Data.Models
{
    public class MedicalImage
    {
        public int MedicalImageId { get; set; }
        public byte[] Image { get; set; } = null!;
        public DateTime UploadDate { get; set; }

        public string Pid { get; set; } = null!;
        public Patient Patient { get; set; } = null!;
        public string Did { get; set; } = null!;
        public Doctor Doctor { get; set; } = null!;
        public AiAnalysis AiAnalysis { get; set; } = null!;
    }
}
