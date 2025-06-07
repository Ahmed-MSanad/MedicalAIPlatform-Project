using AutoMapper;
using MedicalProj.Data.Models;
using Shared.AuthenticationDtos;
using Shared.PatientDtos;

namespace Services.MappingProfiles
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<Patient, PatientDto>()
            .ForMember(pdto => pdto.Gender, options => options.MapFrom(p => p.Gender))
            .ForMember(pdto => pdto.PatientPhones, options => options.MapFrom(p => p.PatientPhones.Select(pp => pp.Phone).ToList()));

            CreateMap<PatientDto, Patient>()
                .ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender))
                .ForMember(dest => dest.PatientPhones, options => options.MapFrom((src, dest) => src.PatientPhones.Select(phone => new PatientPhone
                {
                    Phone = phone
                }).ToList()))
                .ForMember(dest => dest.Image, options => options.Ignore())
                .AfterMap((src, dest) =>
                {
                    try // this works for testing on swagger -> if not over the front end we'll return it back to byte[]
                    {
                        dest.Image = string.IsNullOrEmpty(src.Image)
                            ? null
                            : Convert.FromBase64String(src.Image);
                    }
                    catch (FormatException)
                    {
                        throw new Exception("Invalid base64 string for image in Edit Patient Profile Service");
                    }
                });

            CreateMap<PatientRegistrationModel, Patient>()
                .ForMember(p => p.UserName, options => options.MapFrom(prm => prm.Email))
                .ForMember(p => p.PhoneNumber, options => options.MapFrom(prm => prm.Phones.FirstOrDefault() ?? string.Empty))
                .ForMember(p => p.PatientPhones, options => options.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.PatientPhones = src.Phones.Select(phone => new PatientPhone
                    {
                        Phone = phone,
                        PatientId = dest.Id
                    }).ToList();
                });
        }
    }
}
