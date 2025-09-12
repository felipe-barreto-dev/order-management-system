using Microsoft.EntityFrameworkCore;
using order_management_system.Enums;

namespace order_management_system.Services;

public class OrderService
{
  private readonly ApplicationDbContext _context;

  public OrderService(ApplicationDbContext context)
  {
    _context = context;
  }

  public async Task<List<Order>> GetAllOrdersAsync()
  {
    return await _context.Orders.ToListAsync();
  }

  public async Task<Order?> GetOrderByIdAsync(Guid id)
  {
    return await _context.Orders.FindAsync(id);
  }

  public async Task<Order> CreateOrderAsync(Order order)
  {
    _context.Orders.Add(order);
    await _context.SaveChangesAsync();
    return order;
  }

  public async Task<bool> UpdateOrderStatusAsync(Guid id, OrderStatusEnum status)
  {
    var order = await _context.Orders.FindAsync(id);
    if (order == null) return false;

    order.Status = status;
    await _context.SaveChangesAsync();
    return true;
  }

  public async Task<bool> DeleteOrderAsync(Guid id)
  {
    var order = await _context.Orders.FindAsync(id);
    if (order == null) return false;

    _context.Orders.Remove(order);
    await _context.SaveChangesAsync();
    return true;
  }
}