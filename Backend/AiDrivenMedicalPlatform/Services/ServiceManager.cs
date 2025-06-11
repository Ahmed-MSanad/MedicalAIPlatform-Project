using AutoMapper;
using MedicalProj.Data.Contracts;
using MedicalProj.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Abstraction;

namespace Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IFeedbackService> feedbackService;
        private readonly Lazy<INotificationService> notificationService;
        private readonly Lazy<IAuthenticationService> authenticationService;
        private readonly Lazy<IScheduleService> scheduleService;
        private readonly Lazy<IProfileService> profileService;
        private readonly Lazy<IAppointmentService> appointmentService;
        private readonly Lazy<IMedicalImageService> medicalImageService;
        public ServiceManager(IUnitOfWork unitOfWork, IMapper mapper, IOptions<SmtpSettings> _smtpSettings, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager) {
            feedbackService = new Lazy<IFeedbackService>(() => new FeedbackService(unitOfWork, mapper));
            notificationService = new Lazy<INotificationService>(() => new NotificationService(unitOfWork, mapper, _smtpSettings));
            authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(userManager, roleManager, unitOfWork, mapper));
            scheduleService = new Lazy<IScheduleService>(() => new ScheduleService(unitOfWork, mapper));
            profileService = new Lazy<IProfileService>(() => new ProfileService(unitOfWork, mapper, userManager));
            appointmentService = new Lazy<IAppointmentService>(() => new AppointmentService(unitOfWork, mapper, userManager));
            medicalImageService = new Lazy<IMedicalImageService>(() => new MedicalImageService(unitOfWork, mapper));
        }
        public IFeedbackService FeedbackService => feedbackService.Value;

        public INotificationService NotificationService => notificationService.Value;

        public IAuthenticationService AuthenticationService => authenticationService.Value;

        public IScheduleService ScheduleService => scheduleService.Value;

        public IProfileService ProfileService => profileService.Value;

        public IAppointmentService AppointmentService => appointmentService.Value;

        public IMedicalImageService MedicalImageService => medicalImageService.Value;
    }
}
