using AutoMapper;
using MedicalProj.Data.Models;
using Shared.FeedbackDtos;

namespace Services.MappingProfiles
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<FeedbackMessageDto, Feedback>()
                .ForMember(f => f.SubmittedAt, options => options.MapFrom(_ => DateTime.Now));

            CreateMap<FeedbackResponseDto, Feedback>()
                .ForMember(f => f.RespondedAt, options => options.MapFrom(_ => DateTime.Now));

            CreateMap<Feedback, FeedbackDto>();
        }
    }
}
