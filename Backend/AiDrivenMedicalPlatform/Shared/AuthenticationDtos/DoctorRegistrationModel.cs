namespace Shared.AuthenticationDtos
{
    public class DoctorRegistrationModel : UserRegistrationModel
    {
        public string IdentificationNumber { get; set; }
        public string MedicalLicenseNumber { get; set; }
        public string Specialisation { get; set; }
        public string WorkPlace { get; set; }
        public decimal Fee { get; set; }
    }
}
