using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.DTOs.Common;
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

        public async Task<PagedResult<SchoolClass>> GetAllAsync(PaginationRequest request)
        {
            var query = _context.SchoolClasses
                .OrderBy(c => c.Id);

            var totalCount = await query.CountAsync();

            var classes = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<SchoolClass>
            {
                Items = classes,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<int> CountAsync()
        {
            return await _context.SchoolClasses.CountAsync();
        }

        public async Task<IEnumerable<SchoolClass>> GetAllUnpagedAsync()
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

        public async Task<bool> NameExistsAsync(string name, int? excludeClassId = null)
        {
            return await _context.SchoolClasses
                .AnyAsync(c =>
                    c.Name == name &&
                    (!excludeClassId.HasValue ||
                     c.Id != excludeClassId.Value));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<SchoolClass?> GetClassDetailsAsync(int classId)
        {
            return await _context.SchoolClasses
                .Include(c => c.StudentClasses)
                    .ThenInclude(sc => sc.Student)
                .Include(c => c.TeachingAssignments)
                    .ThenInclude(ta => ta.Subject)
                .Include(c => c.TeachingAssignments)
                    .ThenInclude(ta => ta.Teacher)
                .FirstOrDefaultAsync(c => c.Id == classId);
        }
    }
}
