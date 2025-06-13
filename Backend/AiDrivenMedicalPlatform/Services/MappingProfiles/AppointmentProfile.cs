using AutoMapper;
using MedicalProj.Data.Models;
using shared.AdminDtos;
using Shared.AiAnalysisDtos;
using Shared.AppointmentDtos;

namespace Services.MappingProfiles
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<CreatedAppointmentDto, Appointment>();

            CreateMap<Appointment, AppointmentDto>()
                .ForMember(adto => adto.DoctorName, options => options.MapFrom(a => !string.IsNullOrEmpty(a.Doctor.FullName) ? a.Doctor.FullName : string.Empty))
                .ForMember(adto => adto.PatientName, options => options.MapFrom(a => !string.IsNullOrEmpty(a.Patient.FullName) ? a.Patient.FullName : string.Empty))
                .ForMember(adto => adto.Id, options => options.MapFrom((src, dest) => src.AppointmentId));

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
