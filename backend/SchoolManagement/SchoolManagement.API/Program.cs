
using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Data;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;
using SchoolManagement.API.Mappings;
using SchoolManagement.API.Repositories;
using SchoolManagement.API.Services;

namespace SchoolManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
            ));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

            builder.Services.AddAutoMapper(
                cfg => { },
                typeof(UserProfile).Assembly
            );

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
            builder.Services.AddScoped<ISchoolClassRepository, SchoolClassRepository>();
            builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<ISchoolClassService, SchoolClassService>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
