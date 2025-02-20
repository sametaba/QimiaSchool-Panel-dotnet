using MediatR;
using QimiaSchool.Business.Implementations.Commands.Courses.Dtos;

namespace QimiaSchool.Business.Implementations.Commands.Courses;

public class UpdateCourseCommand : IRequest
{
    public int Id { get; }
    public UpdateCourseDto Course { get; }

    public UpdateCourseCommand(int id, UpdateCourseDto course)
    {
        Id = id;
        Course = course;
    }
}
