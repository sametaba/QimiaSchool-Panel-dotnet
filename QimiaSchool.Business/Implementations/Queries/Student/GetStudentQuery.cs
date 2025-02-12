using MediatR;
using QimiaSchool.Business.Implementations.Queries.Student.Dtos;

namespace QimiaSchool.Business.Implementations.Queries.Student;

    public class GetStudentQuery : IRequest<StudentDto>
    {
        public int Id { get; }  

        public GetStudentQuery(int id) 
        {
            Id = id;
        }
    }


