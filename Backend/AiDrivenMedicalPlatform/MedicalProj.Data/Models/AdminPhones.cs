namespace MedicalProj.Data.Models
{
    public class AdminPhones
    {
        public string Phone { get; set; } = string.Empty;
        public string AdminId { get; set; }

        // EF Core will create a foreign key for this property -> Navigation properties :
        public Admin Admin { get; set; } = null!;
    }
}
