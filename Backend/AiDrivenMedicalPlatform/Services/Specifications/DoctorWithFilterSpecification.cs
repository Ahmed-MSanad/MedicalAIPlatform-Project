using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;
using Shared.DoctorDtos;

namespace Services.Specifications
{
    public class DoctorWithFilterSpecification : Specification<Doctor>
    {
        public DoctorWithFilterSpecification(string doctorId) : base(doctor => doctor.Id == doctorId)
        {
            AddInclude(doctor => doctor.DoctorPhones);
            AddInclude(doctor => doctor.DoctorSchedule);
        }

        public DoctorWithFilterSpecification(DoctorSpecificationParams specification) 
            : base(doctor => (string.IsNullOrWhiteSpace(specification.Speciality) || doctor.Specialisation.ToLower().Contains(specification.Speciality.ToLower())) && 
                             (string.IsNullOrWhiteSpace(specification.Name) || doctor.FullName.ToLower().Contains(specification.Name.ToLower())) &&
                             (!specification.MinRate.HasValue || doctor.Rate >= specification.MinRate) &&
                             (!specification.Cost.HasValue || doctor.Fee <= specification.Cost) &&
                             (string.IsNullOrWhiteSpace(specification.Workplace) || doctor.WorkPlace.ToLower().Contains(specification.Workplace.ToLower()))
            )
        {

        }
    }
}
