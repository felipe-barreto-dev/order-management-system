namespace OrderManagementSystem.Repositories.Base;

public interface IBaseRepository<T, TKey> where T : class
{
    Task<T?> GetByIdAsync(TKey id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(TKey id);
    Task<int> SaveChangesAsync();
}