using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;

namespace Services.Specifications
{
    public class DoctorPhonesWithFilterSpecification : Specification<DoctorPhone>
    {
        public DoctorPhonesWithFilterSpecification(string doctorId) : base(phone => phone.DoctorId == doctorId)
        {
        }
    }
}
