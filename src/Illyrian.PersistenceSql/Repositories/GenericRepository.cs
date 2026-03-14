using Illyrian.Domain.Repositories;
using Illyrian.PersistenceSql.Context;
using Microsoft.EntityFrameworkCore;

namespace Illyrian.PersistenceSql.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly IllyrianDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(IllyrianDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(object id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public virtual async Task<bool> ExistsAsync(object id)
    {
        return await _dbSet.FindAsync(id) != null;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
