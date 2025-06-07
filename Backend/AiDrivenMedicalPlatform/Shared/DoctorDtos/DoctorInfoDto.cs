using Shared.ScheduleDtos;

namespace Shared.DoctorDtos
{
    public class DoctorInfoDto
    {
        public string FullName { get; set; }
        public string Specialisation { get; set; }
        public string WorkPlace { get; set; }
        public decimal Fee { get; set; }
        public decimal? Rate { get; set; }
        public byte[]? Image { get; set; }
        public IEnumerable<ScheduleDto> DoctorSchedule { get; set; } = new List<ScheduleDto>();
        public IEnumerable<string> DoctorPhones { get; set; } = new List<string>();
    }

}
