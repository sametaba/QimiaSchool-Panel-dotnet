using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using QimiaSchool.DataAccess.Entities;


namespace QimiaSchool.DataAccess;
public class QimiaSchoolDbContext : DbContext
{
    public QimiaSchoolDbContext(
    DbContextOptions<QimiaSchoolDbContext> contextOptions) : base(contextOptions)
    {
    }
    public DbSet<Student> Students { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Course> Courses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}