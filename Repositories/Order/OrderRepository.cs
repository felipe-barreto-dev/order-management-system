using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.Data;
using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Base;

namespace OrderManagementSystem.Repositories.Order;

public class OrderRepository(ApplicationDbContext context) : BaseRepository<OrderModel, Guid>(context), IOrderRepository
{
  public async Task<bool> UpdateOrderStatusAsync(Guid id, OrderStatusEnum status)
    {
        var order = await GetByIdAsync(id);
        if (order == null) return false;

        order.Status = status;
        await SaveChangesAsync();
        return true;
    }

    public override async Task<IEnumerable<OrderModel>> GetAllAsync()
    {
        return await _dbSet
            .OrderByDescending(o => o.OrderDate)
            .ToListAsync();
    }

}