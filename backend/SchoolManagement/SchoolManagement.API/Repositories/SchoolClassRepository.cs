using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Repositories;

namespace SchoolManagement.API.Repositories
{
    public class SchoolClassRepository : ISchoolClassRepository
    {
        private readonly ApplicationDbContext _context;

        public SchoolClassRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SchoolClass>> GetAllAsync()
        {
            return await _context.SchoolClasses
                .ToListAsync();
        }

        public async Task<SchoolClass?> GetByIdAsync(int id)
        {
            return await _context.SchoolClasses
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(SchoolClass schoolClass)
        {
            await _context.SchoolClasses.AddAsync(schoolClass);
        }

        public void Update(SchoolClass schoolClass)
        {
            _context.SchoolClasses.Update(schoolClass);
        }

        public void Delete(SchoolClass schoolClass)
        {
            _context.SchoolClasses.Remove(schoolClass);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
