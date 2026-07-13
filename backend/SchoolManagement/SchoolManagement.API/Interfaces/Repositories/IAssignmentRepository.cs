using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface IAssignmentRepository
    {
        Task AddStudentClassAsync(StudentClass studentClass);

        Task AddTeacherSubjectAsync(TeacherSubject teacherSubject);

        Task AddTeachingAssignmentAsync(TeachingAssignment assignment);


        void DeleteStudentClass(StudentClass studentClass);

        void DeleteTeacherSubject(TeacherSubject teacherSubject);

        void DeleteTeachingAssignment(TeachingAssignment assignment);


        Task<StudentClass?> GetStudentClassAsync(int studentId, int classId);

        Task<TeacherSubject?> GetTeacherSubjectAsync(int teacherId, int subjectId);

        Task<TeachingAssignment?> GetTeachingAssignmentAsync(int classId, int subjectId, int teacherId);

        Task<bool> TeacherCanTeachSubjectAsync(int teacherId, int subjectId);

        Task<bool> TeachingAssignmentExistsForClassAsync(int classId, int subjectId);

        Task<bool> StudentAlreadyAssignedToClassAsync(int studentId);
        Task SaveChangesAsync();
    }
}
