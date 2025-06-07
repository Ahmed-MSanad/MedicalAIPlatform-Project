using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;

namespace Services.Specifications
{
    public class PatientWithFilterSpecification : Specification<Patient>
    {
        public PatientWithFilterSpecification(string patientId) : base(patient => patient.Id == patientId)
        {
            AddInclude(patient => patient.PatientPhones);
        }
    }
}
