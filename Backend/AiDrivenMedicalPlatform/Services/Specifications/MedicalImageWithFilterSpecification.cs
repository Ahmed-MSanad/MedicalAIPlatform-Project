using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
