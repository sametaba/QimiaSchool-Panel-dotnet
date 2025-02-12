using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QimiaSchool.DataAccess.Entities;
using QimiaSchool.DataAccess.Repositories.Abstractions;

namespace QimiaSchool.DataAccess.Repositories.Implementations;
public class StudentRepository : RepositoryBase<Student>, IStudentRepository
{
    public StudentRepository(QimiaSchoolDbContext dbContext) : base(dbContext)
    {
    }

    public async Task CreateAsync(Student entity, CancellationToken cancellationToken)
    {
        await DbContext.Students.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    

    void IRepositoryBase<Student>.DeleteAsync(Student entity)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<Student>> IRepositoryBase<Student>.GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task<IEnumerable<Student>> IRepositoryBase<Student>.GetByConditionAsync(Expression<Func<Student, bool>> expression)
    {
        throw new NotImplementedException();
    }

    Task<Student> IRepositoryBase<Student>.GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    Task IRepositoryBase<Student>.UpdateAsync(Student entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
