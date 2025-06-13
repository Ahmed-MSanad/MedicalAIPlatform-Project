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
                }).ToList()));

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