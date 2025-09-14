using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;

namespace OrderManagement.Tests.TestHelpers;

/// <summary>
/// Classe base para testes que precisam de banco de dados
/// Configura automaticamente InMemory database único para cada teste
/// </summary>
public abstract class DatabaseTestBase : IDisposable
{
    protected readonly ApplicationDbContext Context;
    private readonly string _databaseName;

    protected DatabaseTestBase()
    {
        _databaseName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: _databaseName)
            .Options;

        Context = new ApplicationDbContext(options);
    }

    /// <summary>
    /// Método helper para adicionar dados de teste no contexto
    /// </summary>
    protected async Task SeedDataAsync<T>(params T[] entities) where T : class
    {
        Context.Set<T>().AddRange(entities);
        await Context.SaveChangesAsync();
    }

    /// <summary>
    /// Método helper para limpar dados específicos durante o teste
    /// </summary>
    protected async Task ClearDataAsync<T>() where T : class
    {
        Context.Set<T>().RemoveRange(Context.Set<T>());
        await Context.SaveChangesAsync();
    }

    public virtual void Dispose()
    {
        Context?.Dispose();
    }
}