using Microsoft.Extensions.DependencyInjection;
using MediatR;
using AutoMapper;
using QimiaSchool.Business.Abstracts; // IStudentManager burada
using QimiaSchool.Business.Implementations;
using QimiaSchool.Business.Implementations.MapperProfiles; // StudentManager burada

namespace QimiaSchool.Business
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
        {
            // MediatR kayıtlarını ekle
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));

            // AutoMapper kayıtlarını ekle
            services.AddAutoMapper(typeof(MapperProfile));

            // İş katmanı servislerini ekle
            services.AddScoped<IStudentManager, StudentManager>();

            return services;
        }
    }
}
