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

        Task<bool> HasTeachingAssignmentAsync(int teacherId, int subjectId);

        Task<bool> TeacherHasClassAsync(int teacherId, int classId);

        Task<bool> StudentHasClassAssignmentAsync(int userId);

        Task<bool> TeacherHasSubjectAssignmentsAsync(int userId);

        Task<bool> TeacherHasTeachingAssignmentsAsync(int userId);

        Task<bool> SubjectHasTeacherAssignmentsAsync(int subjectId);

        Task<bool> SubjectHasTeachingAssignmentsAsync(int subjectId);

        Task<bool> ClassHasStudentsAsync(int classId);

        Task<bool> ClassHasTeachingAssignmentsAsync(int classId);

        Task<IEnumerable<StudentClass>> GetStudentClassAssignmentsAsync();

        Task<IEnumerable<TeacherSubject>> GetTeacherSubjectAssignmentsAsync();

        Task<IEnumerable<TeachingAssignment>> GetTeachingAssignmentsAsync();

        Task SaveChangesAsync();
    }
}
