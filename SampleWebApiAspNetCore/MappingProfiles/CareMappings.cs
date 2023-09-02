using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class CareMappings : Profile
    {
        public CareMappings()
        {
            CreateMap<CareEntity, CareDto>().ReverseMap();
            CreateMap<CareEntity, CareUpdateDto>().ReverseMap();
            CreateMap<CareEntity, CareCreateDto>().ReverseMap();
        }
    }
}
