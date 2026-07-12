using SchoolManagement.API.DTOs.Users;

namespace SchoolManagement.API.DTOs.SchoolClasses
{
    public class ClassDetailsDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<UserDto> Students { get; set; }

        public List<StudentSubjectDto> Subjects { get; set; }
    }
}
