using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QimiaSchool.DataAccess.Entities;
using QimiaSchool.DataAccess.Repositories.Abstractions;

namespace QimiaSchool.DataAccess.Repositories.Implementations
{
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(QimiaSchoolDbContext dbContext) : base(dbContext)
        {
        }

        public async Task CreateAsync(Course entity, CancellationToken cancellationToken)
        {
            await DbContext.Courses.AddAsync(entity, cancellationToken);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        // Eksik olan DeleteAsync metodu eklendi!
        public async Task DeleteAsync(Course entity)
        {
            DbContext.Courses.Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task<Course> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await DbContext.Courses.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IEnumerable<Course>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbContext.Courses.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Course>> GetByConditionAsync(Expression<Func<Course, bool>> expression)
        {
            return await DbContext.Courses.Where(expression).ToListAsync();
        }

        public async Task UpdateAsync(Course entity, CancellationToken cancellationToken)
        {
            DbContext.Courses.Update(entity);
            await DbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByIdAsync(int courseId, CancellationToken cancellationToken)
        {
            var course = await DbContext.Courses.FindAsync(new object[] { courseId }, cancellationToken);
            if (course != null)
            {
                DbContext.Courses.Remove(course);
                await DbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
