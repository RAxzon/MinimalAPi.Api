using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
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

        public class StudentDbContextFactory : IDesignTimeDbContextFactory<StudentDbContext>
        {
            public StudentDbContext CreateDbContext(string[] args)
            {
                // Get environment
                //var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

                // Build config
                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();

                // Get connection string
                var optionBuilder = new DbContextOptionsBuilder<StudentDbContext>();
                var connectionstring = config.GetConnectionString("SchoolDbConnection");
                optionBuilder.UseSqlServer(connectionstring);
                return new StudentDbContext(optionBuilder.Options);
            }
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

    }
}
