namespace Shared.AuthenticationDtos
{
    public class AdminRegistrationModel : UserRegistrationModel
    {
        public string IdentificationNumber { get; set; }
        public string MedicalLicenseNumber { get; set; }
        public string Specialisation { get; set; }

    }
}
