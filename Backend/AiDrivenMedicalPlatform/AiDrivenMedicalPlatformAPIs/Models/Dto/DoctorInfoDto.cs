using MedicalProj.Data.Models;

namespace AiDrivenMedicalPlatformAPIs.Models.Dto
{
    public class DoctorInfoDto
    {
        public string FullName { get; set; }
        public string Specialisation { get; set; }
        public string WorkPlace { get; set; }
        public decimal Fee { get; set; }
        public decimal? Rate { get; set; }
        public byte[]? Image { get; set; }
        public DoctorScheduleDto DoctorSchedule { get; set; }
        public ICollection<string> DoctorPhones { get; set; } = new List<string>();
    }
    public class DoctorScheduleDto
    {
        public string Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }

}
