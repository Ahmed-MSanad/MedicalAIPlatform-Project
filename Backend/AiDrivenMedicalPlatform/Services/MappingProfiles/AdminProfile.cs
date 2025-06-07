using AutoMapper;
using MedicalProj.Data.Models;
using shared.AdminDtos;
using Shared.AuthenticationDtos;

namespace Services.MappingProfiles
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {
            CreateMap<Admin, AdminDto>()
            .ForMember(pdto => pdto.Gender, options => options.MapFrom(p => p.Gender))
            .ForMember(pdto => pdto.AdminPhones, options => options.MapFrom(p => p.AdminPhones.Select(pp => pp.Phone).ToList()));

            CreateMap<AdminDto, Admin>()
                .ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender))
                .ForMember(dest => dest.AdminPhones, options => options.MapFrom((src, dest) => src.AdminPhones.Select(phone => new AdminPhone
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
                        throw new Exception("Invalid base64 string for image in Edit Admin Profile Service");
                    }
                });

            CreateMap<AdminRegistrationModel, Admin>()
                .ForMember(a => a.UserName, options => options.MapFrom(arm => arm.Email))
                .ForMember(a => a.PhoneNumber, options => options.MapFrom(arm => arm.Phones.FirstOrDefault()))
                .AfterMap((src, dest) =>
                {
                    dest.AdminPhones = src.Phones.Select((phone) => new AdminPhone()
                    {
                        Phone = phone,
                        AdminId = dest.Id
                    }).ToList();
                });
        }
    }
}