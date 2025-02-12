using MediatR;
using System.Collections.Generic;
using QimiaSchool.Business.Implementations.Queries.Student.Dtos;

namespace QimiaSchool.Business.Implementations.Queries.Student
{
    public class GetStudentsQuery : IRequest<List<StudentDto>> 
    {
    }
}
