using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Interfaces.Repositories;

namespace SchoolManagement.API.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;


        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<RefreshToken?> GetByTokenHashAsync(string tokenHash)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(
                    rt => rt.TokenHash == tokenHash);
        }


        public async Task AddAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens
                .AddAsync(refreshToken);
        }


        public void Update(RefreshToken refreshToken)
        {
            _context.RefreshTokens
                .Update(refreshToken);
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
