using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MedicalProj.Data.Models;
using Shared.MedicalImageDtos;

namespace Services.MappingProfiles
{
    public class MedicalImageProfile : Profile
    {
        public MedicalImageProfile()
        {
            CreateMap<CreatedMedicalImageDto, MedicalImage>();

            CreateMap<MedicalImage, MedicalImageDto>();
        }
    }
}
