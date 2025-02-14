using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using QimiaSchool.DataAccess.Entities;

namespace QimiaSchool.DataAccess.Repositories.Abstractions
{
    public interface ICourseRepository : IRepositoryBase<Course>
    {
        Task DeleteByIdAsync(int courseId, CancellationToken cancellationToken);

    }
}
