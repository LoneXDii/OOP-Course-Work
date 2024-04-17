using Microsoft.EntityFrameworkCore;
using Server.Infrastructure.Persistence.Data;
using System.Linq.Expressions;
using System.Linq;

namespace Server.Infrastructure.Persistence.Repositories;

internal class EfRepository<T> : IRepository<T> where T : Entity
{
    protected readonly AppDbContext _dbContext;
    protected readonly DbSet<T> _entities;

    public EfRepository(AppDbContext context)
    {
        _dbContext = context;
        _entities = context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includedProperties)
    {
        IQueryable<T>? query = _entities.AsQueryable();
        if (includedProperties.Any())
        {
            foreach (var property in includedProperties)
            {
                query = query.Include(property);
            }
        }

        return await query.ElementAtAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        return await _entities.AsQueryable().ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? filter,
        CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includedProperties)
    {
        IQueryable<T>? query = _entities.AsQueryable();
        if (includedProperties.Any())
        {
            foreach (var property in includedProperties)
            {
                query = query.Include(property);
            }
        }

        if (filter != null)
        {
            query = query.Where(filter);
        }

        return await query.ToListAsync();
    }

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<T>().AddAsync(entity);
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _entities.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> filter,
        CancellationToken cancellationToken = default)
    {
        return await _entities.FirstOrDefaultAsync(filter, cancellationToken);
    }
}
