using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;

namespace Services.Specifications
{
    public class NotificationWithFilterSpecification : Specification<Notification>
    {
        public NotificationWithFilterSpecification(string PatientId) : base(notification => notification.PatientId == PatientId)
        {
            setOrderByDescending(notification => notification.SubmittedAt);
        }
    }
}
