namespace MedicalProj.Data.Models
{
    public class PatientPhone
    {
        public string Phone { get; set; } = string.Empty;
        public string PatientId { get; set; }

        // EF Core will create a foreign key for this property -> Navigation properties :
        public Patient Patient { get; set; } = null!;
    }
}
