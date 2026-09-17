using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Stationery.Data;

namespace Stationery.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly StationeryDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(StationeryDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task<IReadOnlyList<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<IReadOnlyList<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }

    public void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }
}
