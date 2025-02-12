using QimiaSchool.DataAccess.Entities;

namespace QimiaSchool.Business.Abstracts;
public interface IStudentManager
{
    public Task CreateStudentAsync(
    Student student,
    CancellationToken cancellationToken);
    public Task<Student> GetStudentByIdAsync(
    int studentId,
    CancellationToken cancellationToken);
}

