using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;

namespace Services.Specifications
{
    public class ScheduleWithFilterSpecification : Specification<DoctorSchedule>
    {
        public ScheduleWithFilterSpecification(string doctorId) : base(schedule => schedule.DoctorId == doctorId)
        {
            setOrderBy(schedule => schedule.Day);
        }

        public ScheduleWithFilterSpecification(string id, string dayOfWeek) : base(schedule => schedule.DoctorId == id && schedule.Day == dayOfWeek)
        {

        }
    }
}
