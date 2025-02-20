using MediatR;

namespace QimiaSchool.Business.Implementations.Commands.Courses;

public class DeleteCourseCommand : IRequest
{
    public int Id { get; }

    public DeleteCourseCommand(int id)
    {
        Id = id;
    }
}
