using OrderManagementSystem.Models;

namespace OrderManagementSystem.Services
{
    public interface IOrderService
    {
        Task<OrderModel> CreateOrderAsync(OrderModel order);
        Task<OrderModel?> GetOrderByIdAsync(Guid id);
        Task<List<OrderModel>> GetAllOrdersAsync();
        Task<bool> UpdateOrderStatusAsync(Guid id, Enums.OrderStatusEnum status);
        Task<bool> DeleteOrderAsync(Guid id);
    }
}