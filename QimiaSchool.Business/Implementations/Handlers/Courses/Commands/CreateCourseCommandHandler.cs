using MediatR;
using QimiaSchool.Business.Abstracts;
using QimiaSchool.Business.Implementations.Commands.Courses;
using QimiaSchool.Business.Implementations.Events.Courses;
using QimiaSchool.DataAccess.Entities;
using QimiaSchool.DataAccess.MessageBroker.Abstractions;
namespace QimiaSchool.Business.Implementations.Handlers.Courses.Commands;
public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, int>
{
    private readonly ICourseManager _courseManager;
    private readonly IEventBus _eventBus;
    public CreateCourseCommandHandler(
    ICourseManager courseManager,
    IEventBus eventBus)
    {
        _courseManager = courseManager;
        _eventBus = eventBus;
    }
    public async Task<int> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = new Course
        {
            Title = request.Course.Title,
            Credits = request.Course.Credits,
        };
        await _courseManager.CreateCourseAsync(course, cancellationToken);
        await _eventBus.PublishAsync(new CourseCreatedEvent
        {
            Id = course.ID,
            Title = course.Title,
            Credits = course.Credits,
        });
        return course.ID;
    }
}
