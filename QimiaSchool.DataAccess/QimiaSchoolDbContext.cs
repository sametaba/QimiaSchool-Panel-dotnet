using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QimiaSchool.DataAccess.Entities;

namespace QimiaSchool.DataAccess;
public class QimiaSchoolDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public QimiaSchoolDbContext(
        DbContextOptions<QimiaSchoolDbContext> contextOptions,
        IConfiguration configuration) : base(contextOptions)
    {
        _configuration = configuration;
    }

    public DbSet<Student> Students { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Course> Courses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("❌ Connection string is missing in appsettings.json!");
            }

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
