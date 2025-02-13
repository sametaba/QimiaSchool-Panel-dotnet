using AutoMapper;
using QimiaSchool.Business.Implementations.Commands.Students.Dtos;
using QimiaSchool.Business.Implementations.Queries.Student.Dtos;
using QimiaSchool.DataAccess.Entities;

namespace QimiaSchool.Business.Implementations.MapperProfiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // DTO -> Entity Mapping
            CreateMap<CreateStudentDto, Student>();

            // Entity -> DTO Mapping
            CreateMap<Student, StudentDto>();

            // Eğer ters mapping gerekiyorsa:
            CreateMap<StudentDto, Student>();
        }
    }
}
