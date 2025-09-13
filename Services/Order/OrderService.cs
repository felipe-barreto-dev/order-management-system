using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Order;

namespace OrderManagementSystem.Services;

public class OrderService(IOrderRepository orderRepository) : IOrderService
{
  private readonly IOrderRepository _orderRepository = orderRepository;

  public async Task<List<OrderModel>> GetAllOrdersAsync()
  {
    return await _orderRepository.GetAllOrdersAsync();
  }

  public async Task<OrderModel?> GetOrderByIdAsync(Guid id)
  {
    return await _orderRepository.GetOrderByIdAsync(id);
  }

  public async Task<OrderModel> CreateOrderAsync(OrderModel order)
  {
    return await _orderRepository.CreateOrderAsync(order);
  }

  public async Task<bool> UpdateOrderStatusAsync(Guid id, OrderStatusEnum status)
  {
    return await _orderRepository.UpdateOrderStatusAsync(id, status);
  }

  public async Task<bool> DeleteOrderAsync(Guid id)
  {
    return await _orderRepository.DeleteOrderAsync(id);
  }
}