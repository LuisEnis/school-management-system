using Microsoft.EntityFrameworkCore;
using SchoolManagement.API.Entities;

namespace SchoolManagement.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        public DbSet<User> Users { get; set; }

        public DbSet<SchoolClass> SchoolClasses { get; set; }

        public DbSet<Subject> Subjects { get; set; }

        public DbSet<StudentClass> StudentClasses { get; set; }

        public DbSet<TeacherSubject> TeacherSubjects { get; set; }

        public DbSet<TeachingAssignment> TeachingAssignments { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.StudentClasses)
                .WithOne(sc => sc.Student)
                .HasForeignKey(sc => sc.StudentId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<SchoolClass>()
                .HasMany(sc => sc.StudentClasses)
                .WithOne(sc => sc.SchoolClass)
                .HasForeignKey(sc => sc.SchoolClassId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>()
                .HasMany(u => u.TeacherSubjects)
                .WithOne(ts => ts.Teacher)
                .HasForeignKey(ts => ts.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Subject>()
                .HasMany(s => s.TeacherSubjects)
                .WithOne(ts => ts.Subject)
                .HasForeignKey(ts => ts.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<User>()
                .HasMany(u => u.TeachingAssignments)
                .WithOne(ta => ta.Teacher)
                .HasForeignKey(ta => ta.TeacherId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Subject>()
                .HasMany(s => s.TeachingAssignments)
                .WithOne(ta => ta.Subject)
                .HasForeignKey(ta => ta.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<SchoolClass>()
                .HasMany(sc => sc.TeachingAssignments)
                .WithOne(ta => ta.SchoolClass)
                .HasForeignKey(ta => ta.SchoolClassId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RefreshToken>()
                .HasIndex(rt => rt.TokenHash)
                .IsUnique();
        }
    }
}
