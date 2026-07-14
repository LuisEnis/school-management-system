using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Repositories;

namespace SchoolManagement.API.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly ApplicationDbContext _context;

        public AssignmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddStudentClassAsync(StudentClass studentClass)
        {
            await _context.StudentClasses.AddAsync(studentClass);
        }


        public async Task AddTeacherSubjectAsync(TeacherSubject teacherSubject)
        {
            await _context.TeacherSubjects.AddAsync(teacherSubject);
        }


        public async Task AddTeachingAssignmentAsync(TeachingAssignment assignment)
        {
            await _context.TeachingAssignments.AddAsync(assignment);
        }


        public void DeleteStudentClass(StudentClass studentClass)
        {
            _context.StudentClasses.Remove(studentClass);
        }


        public void DeleteTeacherSubject(TeacherSubject teacherSubject)
        {
            _context.TeacherSubjects.Remove(teacherSubject);
        }


        public void DeleteTeachingAssignment(TeachingAssignment assignment)
        {
            _context.TeachingAssignments.Remove(assignment);
        }


        public async Task<StudentClass?> GetStudentClassAsync(
            int studentId,
            int classId)
        {
            return await _context.StudentClasses
                .FirstOrDefaultAsync(x =>
                    x.StudentId == studentId &&
                    x.SchoolClassId == classId);
        }


        public async Task<TeacherSubject?> GetTeacherSubjectAsync(
            int teacherId,
            int subjectId)
        {
            return await _context.TeacherSubjects
                .FirstOrDefaultAsync(x =>
                    x.TeacherId == teacherId &&
                    x.SubjectId == subjectId);
        }


        public async Task<TeachingAssignment?> GetTeachingAssignmentAsync(
            int classId,
            int subjectId,
            int teacherId)
        {
            return await _context.TeachingAssignments
                .FirstOrDefaultAsync(x =>
                    x.SchoolClassId == classId &&
                    x.SubjectId == subjectId &&
                    x.TeacherId == teacherId);
        }

        public async Task<bool> TeacherCanTeachSubjectAsync(int teacherId, int subjectId)
        {
            return await _context.TeacherSubjects
                .AnyAsync(x =>
                    x.TeacherId == teacherId &&
                    x.SubjectId == subjectId);
        }

        public async Task<bool> TeachingAssignmentExistsForClassAsync(int classId, int subjectId)
        {
            return await _context.TeachingAssignments
                .AnyAsync(x =>
                    x.SchoolClassId == classId &&
                    x.SubjectId == subjectId);
        }

        public async Task<bool> StudentAlreadyAssignedToClassAsync(int studentId)
        {
            return await _context.StudentClasses
                .AnyAsync(x =>
                    x.StudentId == studentId);
        }

        public async Task<bool> HasTeachingAssignmentAsync(int teacherId, int subjectId)
        {
            return await _context.TeachingAssignments
                .AnyAsync(x =>
                    x.TeacherId == teacherId &&
                    x.SubjectId == subjectId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
