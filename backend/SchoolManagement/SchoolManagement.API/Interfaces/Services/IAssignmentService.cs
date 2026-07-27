using SchoolManagement.API.DTOs.Assignments;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface IAssignmentService
    {
        Task<IEnumerable<StudentClassAssignmentDto>> GetStudentClassAssignmentsAsync();

        Task<IEnumerable<TeacherSubjectAssignmentDto>> GetTeacherSubjectAssignmentsAsync();

        Task<IEnumerable<TeachingAssignmentDto>> GetTeachingAssignmentsAsync();

        Task<StudentClassAssignmentDto> AssignStudentToClassAsync(CreateStudentClassAssignmentDto dto);

        Task<TeacherSubjectAssignmentDto> AssignTeacherToSubjectAsync(CreateTeacherSubjectAssignmentDto dto);

        Task<TeachingAssignmentDto> AssignTeacherToClassSubjectAsync(CreateTeachingAssignmentDto dto);

        Task<bool> RemoveStudentFromClassAsync(int studentId, int classId);

        Task<bool> RemoveTeacherFromSubjectAsync(int teacherId, int subjectId);

        Task<bool> RemoveTeachingAssignmentAsync(int classId, int subjectId, int teacherId);
    }
}
