using AutoMapper;
using SchoolManagement.API.DTOs.Subjects;
using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Mappings
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            CreateMap<Subject, SubjectDto>();

            CreateMap<CreateSubjectDto, Subject>();

            CreateMap<UpdateSubjectDto, Subject>();
        }
    }
}
