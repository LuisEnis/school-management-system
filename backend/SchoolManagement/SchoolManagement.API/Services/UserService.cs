using AutoMapper;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Enums;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<IEnumerable<UserDto>> GetByRoleAsync(UserRole role)
        {
            var users = await _userRepository.GetByRoleAsync(role);

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDetailsDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return null;

            return _mapper.Map<UserDetailsDto>(user);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var user = _mapper.Map<User>(dto);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            _mapper.Map(dto, user);

            _userRepository.Update(user);

            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            _userRepository.Delete(user);

            await _userRepository.SaveChangesAsync();

            return true;
        }
    }
}
