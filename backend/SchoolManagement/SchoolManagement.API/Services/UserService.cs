using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using SchoolManagement.API.DTOs.Common;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Entities;
using SchoolManagement.API.Enums;
using SchoolManagement.API.Exceptions;
using SchoolManagement.API.Hubs;
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
        private readonly ILogger<UserService> _logger;
        private readonly IHubContext<SchoolHub> _hubContext;

        public UserService(
            IUserRepository userRepository,
            IAssignmentRepository assignmentRepository,
            IPasswordHasherService passwordHasherService,
            IMapper mapper,
            ILogger<UserService> logger,
            IHubContext<SchoolHub> hubContext)
        {
            _userRepository = userRepository;
            _assignmentRepository = assignmentRepository;
            _passwordHasherService = passwordHasherService;
            _mapper = mapper;
            _logger = logger;
            _hubContext = hubContext;
        }

        public async Task<PagedResult<UserDto>> GetAllAsync(UserQueryRequest request)
        {
            var result = await _userRepository.GetAllAsync(request);

            return new PagedResult<UserDto>
            {
                Items = _mapper.Map<IEnumerable<UserDto>>(result.Items),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }

        public async Task<PagedResult<UserDto>> GetByRoleAsync(UserRole role, UserQueryRequest request)
        {
            var result = await _userRepository
                .GetByRoleAsync(role, request);

            return new PagedResult<UserDto>
            {
                Items = _mapper.Map<IEnumerable<UserDto>>(result.Items),
                PageNumber = result.PageNumber,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount
            };
        }

        public async Task<IEnumerable<UserDto>> GetAllByRoleAsync(UserRole role)
        {
            var users = await _userRepository.GetAllByRoleAsync(role);

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

            _logger.LogInformation("User {UserId} ({Email}) was created with role {Role}.", user.Id, user.Email, user.Role);

            var userDto = _mapper.Map<UserDto>(user);

            if (user.Role == UserRole.Student)
            {
                await _hubContext.Clients.All.SendAsync(
                    "StudentCreated",
                    userDto);
            }
            else if (user.Role == UserRole.Teacher)
            {
                await _hubContext.Clients.All.SendAsync(
                    "TeacherCreated",
                    userDto);
            }

            return userDto;
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

            _logger.LogInformation("User {UserId} ({Email}) was updated.", user.Id, user.Email);

            if (user.Role == UserRole.Student)
            {
                var userDto = _mapper.Map<UserDto>(user);

                await _hubContext.Clients.All.SendAsync(
                    "StudentUpdated",
                    userDto);
            }
            else if (user.Role == UserRole.Teacher)
            {
                var userDto = _mapper.Map<UserDto>(user);

                await _hubContext.Clients.All.SendAsync(
                    "TeacherUpdated",
                    userDto);
            }

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

            _logger.LogInformation("User {UserId} ({Email}) was deleted.", user.Id, user.Email);

            if (user.Role == UserRole.Student)
            {
                await _hubContext.Clients.All.SendAsync(
                    "StudentDeleted",
                    user.Id);
            }
            else if(user.Role == UserRole.Teacher)
            {
                await _hubContext.Clients.All.SendAsync(
                    "TeacherDeleted",
                    user.Id);
            }

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

            _logger.LogInformation("Password changed successfully for user {UserId}.", userId);
        }
    }
}
