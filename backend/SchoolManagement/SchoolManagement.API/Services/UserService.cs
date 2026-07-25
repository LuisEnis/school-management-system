using AutoMapper;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Enums;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;

namespace SchoolManagement.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAssignmentRepository _assignmentRepository;
        private readonly IPasswordHasherService _passwordHasherService;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            IAssignmentRepository assignmentRepository,
            IPasswordHasherService passwordHasherService,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _assignmentRepository = assignmentRepository;
            _passwordHasherService = passwordHasherService;
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

        public async Task<UserDto> CreateAsync(CreateUserDto dto, UserRole currentUserRole)
        {
            if (currentUserRole == UserRole.Secretary && dto.Role != UserRole.Student)
            {
                throw new ForbiddenException("Secretaries can only create students.");
            }

            var emailExists =
                await _userRepository
                    .EmailExistsAsync(dto.Email);

            if (emailExists)
            {
                throw new ConflictException(
                    "Email already exists.");
            }

            var user = _mapper.Map<User>(dto);

            user.PasswordHash =
                _passwordHasherService
                    .HashPassword(
                        user,
                        dto.Password);

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<bool> UpdateAsync(int id, UpdateUserDto dto, UserRole currentUserRole)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            if (currentUserRole == UserRole.Secretary && user.Role != UserRole.Student)
            {
                throw new ForbiddenException("Secretaries can only manage students.");
            }

            var emailExists =
                await _userRepository
                    .EmailExistsAsync(
                        dto.Email,
                        id);

            if (emailExists)
            {
                throw new ConflictException(
                    "Email already exists.");
            }

            _mapper.Map(dto, user);

            _userRepository.Update(user);

            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id, UserRole currentUserRole)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
                return false;

            if (currentUserRole == UserRole.Secretary && user.Role != UserRole.Student)
            {
                throw new ForbiddenException("Secretaries can only delete students.");
            }

            if (user.Role == UserRole.Student)
            {
                var hasClass =
                    await _assignmentRepository
                        .StudentHasClassAssignmentAsync(id);

                if (hasClass)
                {
                    throw new ConflictException(
                        "Cannot delete student because they are assigned to a class.");
                }
            }

            if (user.Role == UserRole.Teacher)
            {
                var hasSubjects =
                    await _assignmentRepository
                        .TeacherHasSubjectAssignmentsAsync(id);

                var hasTeaching =
                    await _assignmentRepository
                        .TeacherHasTeachingAssignmentsAsync(id);


                if (hasSubjects || hasTeaching)
                {
                    throw new ConflictException(
                        "Cannot delete teacher because they have active assignments.");
                }
            }

            _userRepository.Delete(user);

            await _userRepository.SaveChangesAsync();

            return true;
        }

        public async Task ChangePasswordAsync(int userId, ChangePasswordDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new NotFoundException("User not found.");

            var passwordValid = _passwordHasherService.VerifyPassword(
                user,
                user.PasswordHash,
                dto.CurrentPassword);

            if (!passwordValid)
                throw new BadRequestException("Current password is incorrect.");

            if (dto.NewPassword != dto.ConfirmNewPassword)
                throw new BadRequestException("New passwords do not match.");

            if (dto.CurrentPassword == dto.NewPassword)
                throw new BadRequestException("New password must be different from the current password.");

            user.PasswordHash = _passwordHasherService.HashPassword(user, dto.NewPassword);

            await _userRepository.SaveChangesAsync();
        }
    }
}
