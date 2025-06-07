using AutoMapper;
using MedicalProj.Data.Models;
using Shared.NotificationDtos;

namespace Services.MappingProfiles
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<Notification, NotificationDto>();
        }
    }
}
