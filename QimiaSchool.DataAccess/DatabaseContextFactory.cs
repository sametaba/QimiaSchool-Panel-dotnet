using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace QimiaSchool.DataAccess
{
    public class QimiaSchoolDbContextFactory : IDesignTimeDbContextFactory<QimiaSchoolDbContext>
    {
        public QimiaSchoolDbContext CreateDbContext(string[] args)
        {
            // 🔹 Konfigürasyonu oluştur
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Geçerli çalışma dizininden ayarları oku
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 🔹 Bağlantı dizesini al
            string connectionString = configuration.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<QimiaSchoolDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // 🔹 Hata çözümü: `IConfiguration` parametresini de veriyoruz!
            return new QimiaSchoolDbContext(optionsBuilder.Options, configuration);
        }
    }
}
