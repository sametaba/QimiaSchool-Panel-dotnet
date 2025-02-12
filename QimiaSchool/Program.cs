using Microsoft.EntityFrameworkCore;
using QimiaSchool.Business;
using QimiaSchool.DataAccess;
using QimiaSchool.DataAccess.Repositories.Abstractions;
using QimiaSchool.DataAccess.Repositories.Implementations;
using AutoMapper;
using MediatR;
using QimiaSchool.Business.Abstracts;
using QimiaSchool.Business.Implementations;
using QimiaSchool.Business.Implementations.Commands.Students;

var builder = WebApplication.CreateBuilder(args);

// 🔹 Bağımlılıkları ekleyelim
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger UI için ekledim
builder.Services.AddScoped<IStudentManager, StudentManager>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateStudentCommand).Assembly));



// 🔹 Veritabanı bağlantısı
builder.Services.AddDbContext<QimiaSchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 AutoMapper ekleyelim
builder.Services.AddAutoMapper(typeof(QimiaSchool.Business.Implementations.MapperProfiles.MapperProfile));

// 🔹 MediatR ekleyelim
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(QimiaSchool.Business.Implementations.Handlers.Students.Queries.GetStudentQueryHandler).Assembly));

// 🔹 Repository bağımlılıkları
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// 🔹 Business Layer servislerini ekleyelim
builder.Services.AddBusinessLayer();

var app = builder.Build();

// 🔹 Middleware ayarları
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger'ı aç
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
