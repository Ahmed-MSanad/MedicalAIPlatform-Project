
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;

namespace Services.Specifications
{
    public class MedicalImageWithFilterSpecification : Specification<MedicalImage>
    {
        public MedicalImageWithFilterSpecification(int appointmentId) :
            base(medicalImage => medicalImage.AppointmentId == appointmentId)
        {

        }
    }
}
