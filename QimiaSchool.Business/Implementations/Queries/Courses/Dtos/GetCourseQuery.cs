using MediatR;
using QimiaSchool.Business.Implementations.Queries.Courses.Dtos;

namespace QimiaSchool.Business.Implementations.Queries.Courses;

public class GetCourseQuery : IRequest<CourseDto>
{
    public int Id { get; }

    public GetCourseQuery(int id)
    {
        Id = id;
    }
}
