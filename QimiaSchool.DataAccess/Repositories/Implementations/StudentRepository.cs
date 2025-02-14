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

    

    Task IRepositoryBase<Student>.DeleteAsync(Student entity)
    {
        throw new NotImplementedException();
    }

    public async Task<Student> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await DbContext.Students.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<Student>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await DbContext.Students.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Student>> GetByConditionAsync(Expression<Func<Student, bool>> expression)
    {
        return await DbContext.Students.Where(expression).ToListAsync();
    }

    public async Task UpdateAsync(Student entity, CancellationToken cancellationToken)
    {
        DbContext.Students.Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Student entity) // Hata düzeltildi: void yerine Task döndürdük
    {
        DbContext.Students.Remove(entity);
        await DbContext.SaveChangesAsync();
    }
}
