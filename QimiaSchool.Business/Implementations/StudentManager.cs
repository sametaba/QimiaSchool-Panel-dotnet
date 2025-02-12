using QimiaSchool.Business.Abstracts;
using QimiaSchool.DataAccess.Entities;
using QimiaSchool.DataAccess.Repositories.Abstractions;
namespace QimiaSchool.Business.Implementations;
public class StudentManager : IStudentManager
{
    private readonly IStudentRepository _studentRepository;
    public StudentManager(IStudentRepository studentRepository)
    {
        _studentRepository = studentRepository;
    }
    public async Task CreateStudentAsync(Student student, CancellationToken cancellationToken)
    {
        // No ID should be provided while inserting.
        student.ID = default;
        await _studentRepository.CreateAsync(student, cancellationToken);
    }

    public Task<Student> GetStudentByIdAsync(
    int studentId,
    CancellationToken cancellationToken)
    {
        return _studentRepository.GetByIdAsync(studentId, cancellationToken);
    }
}