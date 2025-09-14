using OrderManagementSystem.Repositories.Order;

namespace OrderManagementSystem.Repositories.UnitOfWork;

/// <summary>
/// Interface do padrão Unit of Work
/// Gerencia transações e garante consistência entre repositories
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IOrderRepository Orders { get; }
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    bool HasActiveTransaction { get; }
}