namespace Shared.AppointmentDtos
{
    public class AppointmentDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Cost { get; set; }
        public string Location { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public bool IsRated { get; set; }
        public string Did { get; set; }
    }
}
