using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QimiaSchool.Business.Implementations.Commands.Courses;
using QimiaSchool.Business.Implementations.Commands.Courses.Dtos;
using QimiaSchool.Business.Implementations.Queries.Courses;
using QimiaSchool.Business.Implementations.Queries.Courses.Dtos;
using Serilog;

namespace QimiaSchool.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CoursesController : Controller
{
    private readonly IMediator _mediator;
    private readonly Serilog.ILogger _courseLogger;

    public CoursesController(IMediator mediator)
    {
        _mediator = mediator;
        _courseLogger = Log.ForContext("SourceContext", typeof(CoursesController).FullName);
    }

    [HttpPost]
    public async Task<ActionResult> CreateCourse(
        [FromBody] CreateCourseDto course,
        CancellationToken cancellationToken)
    {
        _courseLogger.Information("Create course request is accepted. Course: {@course}", course);

        var response = await _mediator.Send(new CreateCourseCommand(course), cancellationToken);

        return CreatedAtAction(
            nameof(GetCourse),
            new { Id = response },
            course);
    }

    [HttpGet("{id}")]
    public Task<CourseDto> GetCourse(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetCourseQuery(id), cancellationToken);
    }

    [HttpGet]
    public Task<List<CourseDto>> GetCourses(CancellationToken cancellationToken)
    {
        return _mediator.Send(new GetCoursesQuery(), cancellationToken);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCourse(
        [FromRoute] int id,
        [FromBody] UpdateCourseDto course,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new UpdateCourseCommand(id, course), cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCourse(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteCourseCommand(id), cancellationToken);
        return NoContent();
    }
}
