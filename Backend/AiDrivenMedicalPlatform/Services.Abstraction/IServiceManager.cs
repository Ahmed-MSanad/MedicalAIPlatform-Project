
namespace Services.Abstraction
{
    public interface IServiceManager
    {
        public IFeedbackService FeedbackService { get; }
        public INotificationService NotificationService { get; }
        public IAuthenticationService AuthenticationService { get; }
        public IScheduleService ScheduleService { get; }
        public IProfileService ProfileService { get; }
        public IAppointmentService AppointmentService { get; }
    }
}
