using SchoolManagement.API.DTOs.Assignments;

namespace SchoolManagement.API.Interfaces.Services
{
    public interface IAssignmentService
    {
        public interface IAssignmentService
        {
            Task AssignStudentToClassAsync(StudentClassAssignmentDto dto);

            Task AssignTeacherToSubjectAsync(TeacherSubjectAssignmentDto dto);

            Task AssignTeacherToClassSubjectAsync(
                TeachingAssignmentDto dto);


            Task<bool> RemoveStudentFromClassAsync(
                int studentId,
                int classId);

            Task<bool> RemoveTeacherFromSubjectAsync(
                int teacherId,
                int subjectId);

            Task<bool> RemoveTeachingAssignmentAsync(
                int classId,
                int subjectId,
                int teacherId);
        }
    }
}
