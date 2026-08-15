
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SchoolManagement.API.Data;
using SchoolManagement.API.Interfaces.Repositories;
using SchoolManagement.API.Interfaces.Services;
using SchoolManagement.API.Mappings;
using SchoolManagement.API.Middleware;
using SchoolManagement.API.Repositories;
using SchoolManagement.API.Services;
using SchoolManagement.API.Settings;
using System.Reflection;
using System.Text;

namespace SchoolManagement.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularClient", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")
            ));

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "School Management API",
                    Version = "v1",
                    Description = "Backend API for the School Management System portfolio project."
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

                var xmlPath =
                    Path.Combine(
                        AppContext.BaseDirectory,
                        xmlFile);

                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token.\n\nExample: Bearer eyJhbGciOi..."
                });

                options.AddSecurityRequirement(document =>
                {
                    return new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] =
                            new List<string>()
                    };
                });
            });

            builder.Services.AddAutoMapper(
                cfg => { },
                typeof(UserProfile).Assembly
            );

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ISubjectRepository, SubjectRepository>();
            builder.Services.AddScoped<ISchoolClassRepository, SchoolClassRepository>();
            builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
            builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ISubjectService, SubjectService>();
            builder.Services.AddScoped<ISchoolClassService, SchoolClassService>();
            builder.Services.AddScoped<IAssignmentService, AssignmentService>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IManagementDashboardService, ManagementDashboardService>();

            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            builder.Services.AddScoped<IJwtService, JwtService>();

            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

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

                options.AddPolicy(
                    "ClassView",
                    policy =>
                        policy.RequireRole(
                            "Director",
                            "Secretary",
                            "Teacher"));
            });

            var app = builder.Build();

            await DatabaseInitializer.InitializeAsync(app.Services);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.DocumentTitle = "School Management API";
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "School Management API v1");
                });
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseCors("AngularClient");

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
