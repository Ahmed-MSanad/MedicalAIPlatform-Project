using AutoMapper;
using MedicalProj.Data.Models;
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

            CreateMap<Appointment, AppointmentInfoDto>()
                .ForMember(adto => adto.DoctorName, options => options.MapFrom(a => !string.IsNullOrEmpty(a.Doctor.FullName) ? a.Doctor.FullName : string.Empty))
                .ForMember(adto => adto.PatientName, options => options.MapFrom(a => !string.IsNullOrEmpty(a.Patient.FullName) ? a.Patient.FullName : string.Empty))
                .ForMember(adto => adto.Id, options => options.MapFrom((src, dest) => src.AppointmentId));
        }
    }
}
