using AutoMapper;
using SchoolManagement.API.DTOs.SchoolClasses;
using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Mappings
{
    public class SchoolClassProfile : Profile
    {
        public SchoolClassProfile()
        {
            CreateMap<SchoolClass, SchoolClassDto>();

            CreateMap<CreateSchoolClassDto, SchoolClass>();

            CreateMap<UpdateSchoolClassDto, SchoolClass>();
        }
    }
}
