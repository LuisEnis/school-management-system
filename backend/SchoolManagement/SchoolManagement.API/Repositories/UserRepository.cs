using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Enums;
using SchoolManagement.API.Interfaces.Repositories;

namespace SchoolManagement.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetByRoleAsync(UserRole role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            return await _context.Users
                .AnyAsync(u =>
                    u.Email == email &&
                    (!excludeUserId.HasValue ||
                     u.Id != excludeUserId.Value));
        }

        public async Task<bool> HasStudentClassAssignmentAsync(int userId)
        {
            return await _context.StudentClasses
                .AnyAsync(sc => sc.StudentId == userId);
        }

        public async Task<bool> HasTeacherSubjectAssignmentsAsync(int userId)
        {
            return await _context.TeacherSubjects
                .AnyAsync(ts => ts.TeacherId == userId);
        }

        public async Task<bool> HasTeachingAssignmentsAsync(int userId)
        {
            return await _context.TeachingAssignments
                .AnyAsync(ta => ta.TeacherId == userId);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
