namespace AiDrivenMedicalPlatformAPIs.Models.Dto
{
    public class ScheduleDto
    {
        public DayOfWeek Day {  get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}
