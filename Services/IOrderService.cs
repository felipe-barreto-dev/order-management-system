using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderModel>> GetAllOrdersAsync();
    Task<OrderModel?> GetOrderByIdAsync(Guid id);
    Task<OrderModel> CreateOrderAsync(OrderModel order);
    Task<bool> UpdateOrderStatusAsync(Guid id, OrderStatusEnum status);
    Task<bool> DeleteOrderAsync(Guid id);

}