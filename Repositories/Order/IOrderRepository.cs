using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Base;

namespace OrderManagementSystem.Repositories.Order;

public interface IOrderRepository : IBaseRepository<OrderModel, Guid>
{
    Task<bool> UpdateOrderStatusAsync(Guid id, OrderStatusEnum status);
}