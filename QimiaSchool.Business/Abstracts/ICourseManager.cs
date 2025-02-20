using QimiaSchool.DataAccess.Entities;

namespace QimiaSchool.Business.Abstracts
{
    public interface ICourseManager
    {
        Task<int> CreateCourseAsync(Course course, CancellationToken cancellationToken);
    }
}
