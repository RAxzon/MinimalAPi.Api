using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Models.Data.Configurations;

namespace Models.Data.DataContext
{
    public class StudentDbContext : IdentityDbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new CourseConfig());
            builder.ApplyConfiguration(new UserRoleConfig());
        }

        DbSet<Course> Courses { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Enrollment> Enrollments { get; set; }

    }
}
