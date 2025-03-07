using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using Microsoft.Extensions.Options;
using QimiaSchool.DataAccess.MessageBroker.Implementations;
using MassTransit;
using QimiaSchool.Business.Implementations.Events.Courses;
using QimiaSchool.DataAccess.MessageBroker.Abstractions;
using QimiaSchool.Business.Implementations.Handlers.Courses;
using QimiaSchool.Common;
using System.Text;
using QimiaSchool.Business.Middleware;

Log.Information("Bu bir test logudur! Serilog -> Elasticsearch - Yeni indeks çalışıyor!");
Serilog.Debugging.SelfLog.Enable(Console.Out);

var builder = WebApplication.CreateBuilder(args);


// 🔹 Serilog Logger'ı ayarlayalım
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()  // Daha detaylı log al
    .Enrich.FromLogContext()
    .Enrich.WithExceptionDetails()
    .WriteTo.Console()
    .WriteTo.File("logs/serilog_debug.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
    {
        IndexFormat = "indexdotnetlog",
        AutoRegisterTemplate = false,
        MinimumLogEventLevel = LogEventLevel.Debug, // Daha detaylı log
        //FailureCallback = e => Console.WriteLine($"❌ Elasticsearch Hatası: {e.Message}"),
        //EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog, // Hata olursa kendi loguna yaz
    })
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(Log.Logger);

// 🔹 Bağımlılıkları ekleyelim
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger UI için ekledim
builder.Services.AddScoped<IStudentManager, StudentManager>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateStudentCommand).Assembly));

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "SampleInstance";
});
builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.Configure<MessageBrokerSettings>(
builder.Configuration.GetSection("MessageBroker"));
builder.Services.AddSingleton(sp =>
sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
builder.Services.AddMassTransit(busConfigurator =>
{
    busConfigurator.SetKebabCaseEndpointNameFormatter();
    busConfigurator.AddConsumer<CourseCreatedEventConsumer>();
    busConfigurator.UsingRabbitMq((context, configurator) =>
    {
        MessageBrokerSettings settings = context.GetRequiredService<MessageBrokerSettings>();
        configurator.Host(new Uri(settings.Host), h =>
        {
            h.Username(settings.UserName);
            h.Password(settings.Password);
        });
        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddScoped<ICourseManager, CourseManager>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEventBus, EventBus>();



builder.Services.Configure<Auth0Configuration>(builder.Configuration.GetSection("Auth0"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Auth0:ClientSecret"]));
    options.Authority = $"{builder.Configuration["Auth0:Domain"]}";
    options.Audience = builder.Configuration["Auth0:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidIssuer = $"https://{builder.Configuration["Auth0:Domain"]}",
        ValidAudience = builder.Configuration["Auth0:Audience"],
    };
});



// 🔹 Veritabanı bağlantısı
builder.Services.AddDbContext<QimiaSchoolDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🔹 AutoMapper ekleyelim
builder.Services.AddAutoMapper(typeof(QimiaSchool.Business.Implementations.MapperProfiles.MapperProfile));

// 🔹 MediatR ekleyelim
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(QimiaSchool.Business.Implementations.Handlers.Students.Queries.GetStudentQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<GetCourseQueryHandler>());

// 🔹 Repository bağımlılıkları
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

// 🔹 Business Layer servislerini ekleyelim
builder.Services.AddBusinessLayer();

// 🔹 Auth0 ayarlarını al
var configuration = builder.Configuration;
var auth0Domain = configuration["Auth0:Domain"];
var auth0Audience = configuration["Auth0:Audience"];

// 🔹 Authentication Middleware
/*builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = $"{auth0Domain}";
    options.Audience = auth0Audience;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name"
    };
});*/

// 🔹 Authorization ekleyelim
builder.Services.AddAuthorization();

// 🔹 Swagger'a Authentication ekleme
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Qimia School", Version = "v1" });

    var securityScheme = new OpenApiSecurityScheme
    {
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
        Scheme = "bearer",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
    {
        { securityScheme, new[] { "Bearer" } }
    };

    c.AddSecurityRequirement(securityRequirement);
});

var app = builder.Build();

// 🔹 Middleware ayarları
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger'ı aç
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔹 Authentication ve Authorization middleware'leri ekleyelim
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<TokenRefreshMiddleware>();
app.MapControllers();
app.Run();
public partial class Program { }


