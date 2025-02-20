using MediatR;
using QimiaSchool.Business.Implementations.Queries.Courses.Dtos;

namespace QimiaSchool.Business.Implementations.Queries.Courses;

public class GetCoursesQuery : IRequest<List<CourseDto>> { }
