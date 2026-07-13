using AutoMapper;
using SchoolManagement.API.DTOs.Assignments;
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
        }
    }
}
