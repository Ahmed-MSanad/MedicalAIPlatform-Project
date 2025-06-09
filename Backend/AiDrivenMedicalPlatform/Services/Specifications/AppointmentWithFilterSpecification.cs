using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;
using Shared.AppointmentDtos;

namespace Services.Specifications
{
    public class AppointmentWithFilterSpecification : Specification<Appointment>
    {
        public AppointmentWithFilterSpecification(int status, string userId) : 
            base(appointment => (appointment.Did == userId || appointment.Pid == userId) && (appointment.Status == (AppointmentStatus)status))
        {
            AddInclude(appointment => appointment.Doctor);
            AddInclude(appointment => appointment.Patient);
        }

        public AppointmentWithFilterSpecification(string id, DateTime startOfDay, DateTime endOfDay) :
            base(a => a.Did == id && a.Date >= startOfDay && a.Date < endOfDay && a.Status == AppointmentStatus.Scheduled)
        {

        }

        public AppointmentWithFilterSpecification(int appointmentId) :
            base(a => a.AppointmentId == appointmentId)
        {
            AddInclude(appointment => appointment.Doctor);
            AddInclude(appointment => appointment.Patient);
        }

    }
}
