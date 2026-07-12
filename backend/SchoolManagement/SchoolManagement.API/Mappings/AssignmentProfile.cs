using AutoMapper;
using SchoolManagement.API.DTOs.Assignments;
using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Mappings
{
    public class AssignmentProfile : Profile
    {
        public AssignmentProfile()
        {
            CreateMap<StudentClassAssignmentDto, StudentClass>();

            CreateMap<TeacherSubjectAssignmentDto, TeacherSubject>();

            CreateMap<TeachingAssignmentDto, TeachingAssignment>();
        }
    }
}
