using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.DTOs.Common;
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

        public async Task<PagedResult<User>> GetAllAsync(UserQueryRequest request)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(u =>
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search) ||
                    u.Email.Contains(search));
            }

            query = request.SortBy?.ToLower() switch
            {
                "fullname" => request.SortDescending
                    ? query.OrderByDescending(u => u.FirstName)
                            .ThenByDescending(u => u.LastName)
                    : query.OrderBy(u => u.FirstName)
                            .ThenBy(u => u.LastName),

                "email" => request.SortDescending
                    ? query.OrderByDescending(u => u.Email)
                    : query.OrderBy(u => u.Email),

                _ => request.SortDescending
                    ? query.OrderByDescending(u => u.Id)
                    : query.OrderBy(u => u.Id)
            };

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = users,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<User>> GetByRoleAsync(UserRole role, UserQueryRequest request)
        {
            var query = _context.Users
                .Where(u => u.Role == role);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim();

                query = query.Where(u =>
                    u.FirstName.Contains(search) ||
                    u.LastName.Contains(search) ||
                    u.Email.Contains(search));
            }

            query = request.SortBy?.ToLower() switch
            {
                "fullname" => request.SortDescending
                    ? query.OrderByDescending(u => u.FirstName)
                            .ThenByDescending(u => u.LastName)
                    : query.OrderBy(u => u.FirstName)
                            .ThenBy(u => u.LastName),

                "email" => request.SortDescending
                    ? query.OrderByDescending(u => u.Email)
                    : query.OrderBy(u => u.Email),

                _ => request.SortDescending
                    ? query.OrderByDescending(u => u.Id)
                    : query.OrderBy(u => u.Id)
            };

            var totalCount = await query.CountAsync();

            var users = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Items = users,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<IEnumerable<User>> GetAllByRoleAsync(UserRole role)
        {
            return await _context.Users
                .Where(u => u.Role == role)
                .OrderBy(u => u.Id)
                .ToListAsync();
        }

        public async Task<int> CountByRoleAsync(UserRole role)
        {
            return await _context.Users
                .CountAsync(u => u.Role == role);
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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
