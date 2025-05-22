namespace MedicalProj.Data.Models
{
    public class DoctorPhone
    {
        public string Phone { get; set; } = string.Empty;
        public string DoctorId { get; set; }

        // EF Core will create a foreign key for this property -> Navigation properties :
        public Doctor Doctor { get; set; } = null!;
    }
}
