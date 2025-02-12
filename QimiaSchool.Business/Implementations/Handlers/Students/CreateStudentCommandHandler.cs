using AutoMapper;
using MediatR;
using QimiaSchool.Business.Abstracts;
using QimiaSchool.Business.Implementations.Commands.Students;
using QimiaSchool.DataAccess.Entities;

public class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, int>
{
    private readonly IStudentManager _studentManager;
    private readonly IMapper _mapper;

    public CreateStudentCommandHandler(IStudentManager studentManager, IMapper mapper)
    {
        _studentManager = studentManager;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
    {
        var student = _mapper.Map<Student>(request.Student);
        await _studentManager.CreateStudentAsync(student, cancellationToken);
        return student.ID;
    }
}
