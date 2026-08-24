using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.DTOs.Common;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Repositories;

namespace SchoolManagement.API.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly ApplicationDbContext _context;

        public SubjectRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Subject>> GetAllAsync(PaginationRequest request)
        {
            var query = _context.Subjects
                .OrderBy(s => s.Id);

            var totalCount = await query.CountAsync();

            var subjects = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<Subject>
            {
                Items = subjects,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<int> CountAsync()
        {
            return await _context.Subjects.CountAsync();
        }

        public async Task<IEnumerable<Subject>> GetAllUnpagedAsync()
        {
            return await _context.Subjects
                .ToListAsync();
        }

        public async Task<Subject?> GetByIdAsync(int id)
        {
            return await _context.Subjects
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task AddAsync(Subject subject)
        {
            await _context.Subjects.AddAsync(subject);
        }

        public void Update(Subject subject)
        {
            _context.Subjects.Update(subject);
        }

        public void Delete(Subject subject)
        {
            _context.Subjects.Remove(subject);
        }

        public async Task<bool> NameExistsAsync(string name, int? excludeSubjectId = null)
        {
            return await _context.Subjects
                .AnyAsync(s =>
                    s.Name == name &&
                    (!excludeSubjectId.HasValue ||
                     s.Id != excludeSubjectId.Value));
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
