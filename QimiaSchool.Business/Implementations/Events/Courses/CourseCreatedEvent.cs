namespace QimiaSchool.Business.Implementations.Events.Courses;
public record CourseCreatedEvent
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public int Credits { get; init; }
}
