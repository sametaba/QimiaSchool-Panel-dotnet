using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using QimiaSchool.Business.Abstracts;
using System.Threading;
using System.Threading.Tasks;
using QimiaSchool.DataAccess.Entities;
using QimiaSchool.DataAccess.Repositories.Abstractions;

namespace QimiaSchool.Business.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<CacheService> _logger;
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICacheService _cacheService;

        public CacheService(IDistributedCache cache, ILogger<CacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var value = await _cache.GetStringAsync(key, cancellationToken);
            if (value != null)
            {
                _logger.LogInformation("Cache hit for key '{Key}'", key);
                return JsonSerializer.Deserialize<T>(value);
            }
            _logger.LogInformation("Cache miss for key '{Key}'", key);
            return default;
        }

        public async Task<bool> SetAsync<T>(string key, T value, TimeSpan? expirationDate = null, CancellationToken cancellationToken = default)
        {
            var options = new DistributedCacheEntryOptions();
            if (expirationDate.HasValue)
            {
                options.AbsoluteExpirationRelativeToNow = expirationDate.Value;
            }
            var serializedValue = JsonSerializer.Serialize(value);
            await _cache.SetStringAsync(key, serializedValue, options, cancellationToken);
            _logger.LogInformation("Cache set for key '{Key}'", key);
            return true;
        }

        public async Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            var exist = await _cache.GetAsync(key, cancellationToken);
            if (exist != null)
            {
                await _cache.RemoveAsync(key, cancellationToken);
                _logger.LogInformation("Cache removed for key '{Key}'", key);
                return true;
            }
            _logger.LogInformation("Cache removal failed for key '{Key}'", key);
            return false;
        }

        public async Task DeleteCourseByIdAsync(int courseId, CancellationToken cancellationToken)
        {
            var cacheKey = $"course-{courseId}";
            var cachedCourse = await _cacheService.GetAsync<Course>(cacheKey, cancellationToken);
            if (cachedCourse != null)
            {
                await _cacheService.RemoveAsync(cacheKey, cancellationToken);
            }
            await _courseRepository.DeleteByIdAsync(courseId, cancellationToken);
        }

        public async Task<Course> GetCourseByIdAsync(int courseId, CancellationToken cancellationToken)
        {
            var cacheKey = $"course-{courseId}";
            var cachedCourse = await _cacheService.GetAsync<Course>(cacheKey, cancellationToken);
            if (cachedCourse != null)
            {
                return cachedCourse;
            }
            var course = await _courseRepository.GetByIdAsync(courseId, cancellationToken);
            await _cacheService.SetAsync(cacheKey, course, TimeSpan.FromMinutes(5), cancellationToken);
            return course;
        }

        public async Task UpdateCourseAsync(Course course, CancellationToken cancellationToken)
        {
            var cacheKey = $"course-{course.ID}";
            var cachedCourse = await _cacheService.GetAsync<Course>(cacheKey, cancellationToken);
            if (cachedCourse != null)
            {
                await _cacheService.RemoveAsync(cacheKey, cancellationToken);
            }
            await _courseRepository.UpdateAsync(course, cancellationToken);
        }

        public async Task<Student> GetStudentByIdAsync(int studentId, CancellationToken cancellationToken)
        {
            var cacheKey = $"student-{studentId}";
            var cachedStudent = await _cacheService.GetAsync<Student>(cacheKey, cancellationToken);
            if (cachedStudent != null)
            {
                return cachedStudent;
            }
            var student = await _studentRepository.GetByIdAsync(studentId, cancellationToken);
            if (student != null)
            {
                await _cacheService.SetAsync(cacheKey, student, TimeSpan.FromMinutes(5), cancellationToken);
            }
            return student;
        }

        public async Task DeleteStudentByIdAsync(int studentId, CancellationToken cancellationToken)
        {
            var cacheKey = $"student-{studentId}";
            var cachedStudent = await _cacheService.GetAsync<Student>(cacheKey, cancellationToken);
            if (cachedStudent != null)
            {
                await _cacheService.RemoveAsync(cacheKey, cancellationToken);
            }
            await _studentRepository.DeleteByIdAsync(studentId, cancellationToken);
        }

        public async Task UpdateStudentAsync(Student student, CancellationToken cancellationToken)
        {
            var cacheKey = $"student-{student.ID}";
            var cachedStudent = await _cacheService.GetAsync<Student>(cacheKey, cancellationToken);
            if (cachedStudent != null)
            {
                await _cacheService.RemoveAsync(cacheKey, cancellationToken);
            }
            await _studentRepository.UpdateAsync(student, cancellationToken);
        }



    }
}
