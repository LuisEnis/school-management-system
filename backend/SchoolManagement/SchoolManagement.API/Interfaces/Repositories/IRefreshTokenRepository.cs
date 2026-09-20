using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Interfaces.Repositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenHashAsync(string tokenHash);

        Task AddAsync(RefreshToken refreshToken);

        void Update(RefreshToken refreshToken);

        Task SaveChangesAsync();
    }
}
