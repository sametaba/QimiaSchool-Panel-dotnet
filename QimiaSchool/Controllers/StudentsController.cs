using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QimiaSchool.Business.Implementations.Commands.Students;
using QimiaSchool.Business.Implementations.Commands.Students.Dtos;
using QimiaSchool.Business.Implementations.Queries.Student;
using QimiaSchool.Business.Implementations.Queries.Student.Dtos;
using static QimiaSchool.Business.Implementations.Queries.Student.GetStudentQuery;
namespace QimiaSchool.Controllers;



[ApiController]
[Authorize]
[Route("[controller]")]
public class StudentsController : Controller
{
    private readonly IMediator _mediator;
    public StudentsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpPost]
    public async Task<ActionResult> CreateStudent(
    [FromBody] CreateStudentDto student,
    CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(new CreateStudentCommand(student), cancellationToken);
        return CreatedAtAction(
        nameof(GetStudent),
        new { Id = response },
        student);
    }
    [HttpGet("{id}")]
    public Task<StudentDto> GetStudent(
    [FromRoute] int id,
    CancellationToken cancellationToken)
    {
        return _mediator.Send(
        new GetStudentQuery(id),
        cancellationToken);
    }
    [HttpGet]
    public Task<List<StudentDto>> GetStudents(CancellationToken cancellationToken)
    {
        return _mediator.Send(
        new GetStudentsQuery(),
        cancellationToken);
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateStudent(
    [FromRoute] int id,
    [FromBody] UpdateStudentDto student,
    CancellationToken cancellationToken)
    {
        await _mediator.Send(
        new UpdateStudentCommand(id, student),
        cancellationToken);
        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteStudent(
    [FromRoute] int id,
    CancellationToken cancellationToken)
    {
        await _mediator.Send(
        new DeleteStudentCommand(id),
        cancellationToken);
        return NoContent();
    }
}
