using AutoMapper;
using SchoolManagement.API.DTOs.Assignments;
using SchoolManagement.API.DTOs.Teacher;
using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Mappings
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<CreateStudentClassAssignmentDto, StudentClass>();

            CreateMap<CreateTeacherSubjectAssignmentDto, TeacherSubject>();

            CreateMap<CreateTeachingAssignmentDto, TeachingAssignment>();

            CreateMap<StudentClass, StudentClassAssignmentDto>();

            CreateMap<TeacherSubject, TeacherSubjectAssignmentDto>();

            CreateMap<TeachingAssignment, TeachingAssignmentDto>();

            CreateMap<TeachingAssignment, TeacherAssignmentDto>()
                .ForMember(dest => dest.ClassId,
                    opt => opt.MapFrom(src => src.SchoolClassId))
                .ForMember(dest => dest.ClassName,
                    opt => opt.MapFrom(src => src.SchoolClass.Name))
                .ForMember(dest => dest.SubjectId,
                    opt => opt.MapFrom(src => src.SubjectId))
                .ForMember(dest => dest.SubjectName,
                    opt => opt.MapFrom(src => src.Subject.Name));
        }
    }
}
