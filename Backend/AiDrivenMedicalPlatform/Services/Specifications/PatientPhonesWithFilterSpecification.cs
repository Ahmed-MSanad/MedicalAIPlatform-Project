using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;

namespace Services.Specifications
{
    public class PatientPhonesWithFilterSpecification : Specification<PatientPhone>
    {
        public PatientPhonesWithFilterSpecification(string patientId)
            : base(phone => phone.PatientId == patientId)
        {

        }
    }
}
