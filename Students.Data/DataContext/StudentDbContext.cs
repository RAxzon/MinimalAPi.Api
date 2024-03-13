using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Models.Data.DataContext
{
    public class StudentDbContext : IdentityDbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
        {}

        DbSet<Course> Courses { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Enrollment> Enrollments { get; set; }

    }
}
