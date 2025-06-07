using AutoMapper;
using MedicalProj.Data.Models;
using Shared.AuthenticationDtos;
using Shared.DoctorDtos;
using Shared.ScheduleDtos;

namespace Services.MappingProfiles
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<DoctorRegistrationModel, Doctor>()
                .ForMember(d => d.UserName, options => options.MapFrom(dr => dr.Email))
                .ForMember(d => d.PhoneNumber, options => options.MapFrom(dr => dr.Phones.FirstOrDefault() ?? string.Empty))
                .AfterMap((src, dest) =>
                {
                    dest.DoctorPhones = src.Phones.Select(phone => new DoctorPhone()
                    {
                        Phone = phone,
                        DoctorId = dest.Id
                    }).ToList();
                });


            CreateMap<Doctor, DoctorDto>()
            .ForMember(ddto => ddto.Gender, options => options.MapFrom(d => d.Gender))
            .ForMember(ddto => ddto.DoctorPhones, options => options.MapFrom(d => d.DoctorPhones.Select(dp => dp.Phone).ToList()));

            CreateMap<DoctorDto, Doctor>()
                .ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender))
                .ForMember(dest => dest.DoctorPhones, options => options.MapFrom((src, dest) => src.DoctorPhones.Select(phone => new DoctorPhone
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
                        throw new Exception("Invalid base64 string for image in Edit Doctor Profile Service");
                    }
                });


            CreateMap<Doctor, DoctorResponseDto>();

            CreateMap<Doctor, DoctorInfoDto>()
                .ForMember(dest => dest.DoctorPhones, options => options.MapFrom((src, dest) => src.DoctorPhones.Select(phone => phone.Phone).ToList()))
                .ForMember(dest => dest.DoctorSchedule, options => options.MapFrom((src, dest) => src.DoctorSchedule.Select((schedule) => new ScheduleDto(){
                    Day = schedule.Day,
                    From = Convert.ToString(schedule.From),
                    To = Convert.ToString(schedule.To),
                })));

            CreateMap<DoctorSchedule, ScheduleDto>()
                .ForMember(dest => dest.From, options => options.MapFrom((src, dest) => Convert.ToString(src.From)))
                .ForMember(dest => dest.To, options => options.MapFrom((src, dest) => Convert.ToString(src.To)));
        }
    }
}
