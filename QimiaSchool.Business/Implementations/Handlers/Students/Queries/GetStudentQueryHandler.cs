using AutoMapper;
using MediatR;
using QimiaSchool.Business.Abstracts;
using QimiaSchool.Business.Implementations.Queries.Student;
using QimiaSchool.Business.Implementations.Queries.Student.Dtos;

namespace QimiaSchool.Business.Implementations.Handlers.Students.Queries;

public class GetStudentQueryHandler : IRequestHandler<GetStudentQuery, StudentDto>
{
    private readonly IStudentManager _studentManager;
    private readonly IMapper _mapper;

    public GetStudentQueryHandler(IStudentManager studentManager, IMapper mapper)
    {
        _studentManager = studentManager;
        _mapper = mapper;
    }

    public async Task<StudentDto> Handle(GetStudentQuery request, CancellationToken cancellationToken)
    {
        var student = await _studentManager.GetStudentByIdAsync(request.Id, cancellationToken);
        return _mapper.Map<StudentDto>(student);
    }
}
