using AutoMapper;
using MedicalProj.Data.Models;
using Shared.AiAnalysisDtos;

namespace Services.MappingProfiles
{
    public class AiAnalysisProfile : Profile
    {
        public AiAnalysisProfile()
        {
            CreateMap<AiAnalysisDto, AiAnalysis>()
                .ForMember(dest => dest.image, options => options.Ignore())
                .AfterMap((src, dest) =>
                {
                    try // this works for testing on swagger -> if not over the front end we'll return it back to byte[]
                    {
                        dest.image = string.IsNullOrEmpty(src.image)
                            ? null
                            : Convert.FromBase64String(src.image);
                    }
                    catch (FormatException)
                    {
                        throw new Exception("Invalid base64 string for image in Edit Admin Profile Service");
                    }
                });

            CreateMap<AiAnalysis, AiAnalysisDto>()
                .ForMember(dest => dest.image, opt => opt.MapFrom(src => src.image != null ? Convert.ToBase64String(src.image) : null));
        }
    }
}
