namespace Shared.AppointmentDtos
{
    public class AppointmentDto
    {
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public string Location { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
    }
}
