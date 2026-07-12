using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Entities;
using AutoMapper;

namespace SchoolManagement.API.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<User, UserDetailsDto>();

            CreateMap<CreateUserDto, User>()
                .ForMember(
                    destination => destination.PasswordHash,
                    options => options.Ignore()
                );

            CreateMap<UpdateUserDto, User>()
                .ForMember(
                    destination => destination.PasswordHash,
                    options => options.Ignore()
                );
        }
    }
}
