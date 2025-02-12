using MediatR;
using QimiaSchool.Business.Implementations.Commands.Students.Dtos;

namespace QimiaSchool.Business.Implementations.Commands.Students
{
    public class UpdateStudentCommand : IRequest
    {
        public int Id { get; set; }
        public UpdateStudentDto Student { get; set; }

        public UpdateStudentCommand(int id, UpdateStudentDto student)
        {
            Id = id;
            Student = student;
        }
    }
}
