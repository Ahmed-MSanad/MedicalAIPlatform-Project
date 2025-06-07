namespace Shared.AuthenticationDtos
{
    public class PatientRegistrationModel : UserRegistrationModel
    {
        public string Occupation { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string FamilyMedicalHistory { get; set; }
        public string PastMedicalHistory { get; set; }
    }
}
