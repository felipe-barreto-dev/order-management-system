using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;

namespace OrderManagementSystem.Repositories.Base;

/// <summary>
/// Implementação base genérica do repository pattern
/// Fornece operações CRUD comuns para qualquer entidade
/// </summary>
/// <typeparam name="T">Tipo da entidade</typeparam>
/// <typeparam name="TKey">Tipo da chave primária</typeparam>
public class BaseRepository<T, TKey> : IBaseRepository<T, TKey> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _dbSet = _context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(TKey id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        var result = await _dbSet.AddAsync(entity);
        return result.Entity;
    }

    public virtual Task<T> UpdateAsync(T entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        _dbSet.Update(entity);
        return Task.FromResult(entity);
    }

    public virtual async Task<bool> DeleteAsync(TKey id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null) return false;

        _dbSet.Remove(entity);
        return true;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}