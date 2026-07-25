using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Repositories;

namespace SchoolManagement.API.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly ApplicationDbContext _context;


        public StudentRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<StudentClass?> GetStudentClassAsync(int studentId)
        {
            return await _context.StudentClasses
                .Include(sc => sc.SchoolClass)
                    .ThenInclude(c => c.TeachingAssignments)
                        .ThenInclude(ta => ta.Subject)

                .Include(sc => sc.SchoolClass)
                    .ThenInclude(c => c.TeachingAssignments)
                        .ThenInclude(ta => ta.Teacher)

                .FirstOrDefaultAsync(sc =>
                    sc.StudentId == studentId);
        }
    }
}
