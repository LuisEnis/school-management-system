using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Repositories;

namespace SchoolManagement.API.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _context;


        public TeacherRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<TeachingAssignment>> GetTeacherClassesAsync(int teacherId)
        {
            return await _context.TeachingAssignments
                .Include(x => x.SchoolClass)
                .Include(x => x.Subject)
                .Where(x => x.TeacherId == teacherId)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetStudentsByClassAsync(int classId)
        {
            return await _context.StudentClasses
                .Where(x => x.SchoolClassId == classId)
                .Select(x => x.Student)
                .ToListAsync();
        }
    }
}
