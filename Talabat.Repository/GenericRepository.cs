using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Infrastructure;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly StoreContext _dbContext;

    public GenericRepository(StoreContext dbContext) // Ask Clr For Creating Object From DbContext
    {
        _dbContext = dbContext;
    }

    public async Task<T?> GetAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T?>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }

    public async Task<T?> GetWithSpecAsync(ISpecifications<T> spec)
    {
        return await ApplySpecifiCations(spec).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<T?>> GetAllWithSpecAsync(ISpecifications<T> spec)
    {
        return await ApplySpecifiCations(spec).ToListAsync();
    }





    private IQueryable<T> ApplySpecifiCations(ISpecifications<T> spec)
    {
        return SpecificationsEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
    }

    public async Task<int> GetCountAsync(ISpecifications<T> spec)
    {
        return await ApplySpecifiCations(spec).CountAsync();
    }

    public void Add(T entity)
    {
        _dbContext.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        _dbContext.Set<T>().Update(entity);
    }

    public void Delete(T entity)
    {
        _dbContext.Set<T>().Remove(entity);
    }
}