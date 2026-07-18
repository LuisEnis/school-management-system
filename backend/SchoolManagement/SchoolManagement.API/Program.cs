
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SchoolManagement.API.Data;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;
using SchoolManagement.API.Mappings;
using SchoolManagement.API.Middleware;
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

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            builder.Services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme =
                        JwtBearerDefaults.AuthenticationScheme;

                    options.DefaultChallengeScheme =
                        JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(
                    options =>
                    {
                        var jwtSettings =
                            builder.Configuration
                                .GetSection("Jwt");


                        options.TokenValidationParameters =
                            new TokenValidationParameters
                            {
                                ValidateIssuer = true,

                                ValidateAudience = true,

                                ValidateLifetime = true,

                                ValidateIssuerSigningKey = true,


                                ValidIssuer =
                                    jwtSettings["Issuer"],

                                ValidAudience =
                                    jwtSettings["Audience"],

                                IssuerSigningKey =
                                    new SymmetricSecurityKey(
                                        Encoding.UTF8.GetBytes(
                                            jwtSettings["Key"]!
                                        ))
                            };
                    });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(
                    "Management",
                    policy =>
                        policy.RequireRole(
                            "Director",
                            "Secretary"));


                options.AddPolicy(
                    "DirectorOnly",
                    policy =>
                        policy.RequireRole(
                            "Director"));


                options.AddPolicy(
                    "TeacherOnly",
                    policy =>
                        policy.RequireRole(
                            "Teacher"));


                options.AddPolicy(
                    "StudentOnly",
                    policy =>
                        policy.RequireRole(
                            "Student"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
