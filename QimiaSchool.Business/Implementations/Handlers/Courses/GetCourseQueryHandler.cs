using MediatR;
using QimiaSchool.Business.Implementations.Queries.Courses;
using QimiaSchool.Business.Implementations.Queries.Courses.Dtos;
using QimiaSchool.DataAccess.Repositories.Abstractions;
using System.Threading;
using System.Threading.Tasks;

namespace QimiaSchool.Business.Implementations.Handlers.Courses
{
    public class GetCourseQueryHandler : IRequestHandler<GetCourseQuery, CourseDto>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCourseQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseDto> Handle(GetCourseQuery request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.Id, cancellationToken);
            if (course == null)
            {
                throw new KeyNotFoundException($"Course with ID {request.Id} not found.");
            }

            return new CourseDto
            {
                Id = course.ID,
                Title = course.Title,
                Credits = course.Credits
            };
        }
    }
}
