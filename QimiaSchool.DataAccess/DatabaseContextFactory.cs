using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace QimiaSchool.DataAccess;
/// <summary>
/// QimiaSchoolDbContextFactory.
/// </summary>
public class QimiaSchoolDbContextFactory : IDesignTimeDbContextFactory<QimiaSchoolDbContext>
{
    public QimiaSchoolDbContext CreateDbContext(string[] args)
    {
        if (args.Length < 1)
        {
            throw new ArgumentException("Missing connection string argument.");
        }
        var connectionString = args[0];
        var builder = new DbContextOptionsBuilder<QimiaSchoolDbContext>()
        .UseSqlServer(connectionString);
        return new QimiaSchoolDbContext(builder.Options);
    }
}
