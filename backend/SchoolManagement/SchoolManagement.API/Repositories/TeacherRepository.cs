using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.DTOs.Teacher;
using SchoolManagement.API.DTOs.Users;
using SchoolManagement.API.Interfaces.Repositories;

namespace SchoolManagement.API.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _context;


        public TeacherRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<TeacherAssignmentDto>> GetTeacherClassesAsync(int teacherId)
        {
            return await _context.TeachingAssignments
                .Where(x => x.TeacherId == teacherId)
                .Select(x => new TeacherAssignmentDto
                {
                    ClassId = x.SchoolClassId,

                    ClassName = x.SchoolClass.Name,

                    SubjectId = x.SubjectId,

                    SubjectName = x.Subject.Name
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<UserDto>> GetStudentsByClassAsync(int classId)
        {
            return await _context.StudentClasses
                .Where(x =>
                    x.SchoolClassId == classId)
                .Select(x => new UserDto
                {
                    Id = x.Student.Id,

                    FirstName = x.Student.FirstName,

                    LastName = x.Student.LastName,

                    Email = x.Student.Email,

                    Role = x.Student.Role
                })
                .ToListAsync();
        }
    }
}
