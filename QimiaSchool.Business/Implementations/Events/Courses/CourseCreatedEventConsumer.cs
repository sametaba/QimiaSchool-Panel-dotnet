using MassTransit;
using Microsoft.Extensions.Logging;
using QimiaSchool.Business.Abstracts;
namespace QimiaSchool.Business.Implementations.Events.Courses;
public class CourseCreatedEventConsumer : IConsumer<CourseCreatedEvent>
{
    private readonly ILogger<CourseCreatedEventConsumer> _logger;
    public CourseCreatedEventConsumer(ILogger<CourseCreatedEventConsumer> logger)
    {
        _logger = logger;
    }
    public Task Consume(ConsumeContext<CourseCreatedEvent> context)
    {
        _logger.LogInformation("Course created: {@Course}", context.Message);
        return Task.CompletedTask;
    }
}
