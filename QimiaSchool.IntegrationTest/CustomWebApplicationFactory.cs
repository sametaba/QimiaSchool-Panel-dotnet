using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QimiaSchool.DataAccess;
namespace QimiaSchool.IntegrationTests;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _connectionString;
    public CustomWebApplicationFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<QimiaSchoolDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            services.AddDbContext<QimiaSchoolDbContext>(options => options.UseSqlServer(_connectionString));
        });
    }
}
