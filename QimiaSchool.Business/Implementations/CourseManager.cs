using QimiaSchool.Business.Abstracts;
using QimiaSchool.DataAccess.Entities;
using QimiaSchool.DataAccess.Repositories.Abstractions;

namespace QimiaSchool.Business.Implementations
{
    public class CourseManager : ICourseManager
    {
        private readonly ICourseRepository _courseRepository;

        public CourseManager(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<int> CreateCourseAsync(Course course, CancellationToken cancellationToken)
        {
            await _courseRepository.CreateAsync(course, cancellationToken);
            return course.ID;
        }
    }
}
