using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.UnitOfWork;

namespace OrderManagementSystem.Services;

public class OrderService(IUnitOfWork unitOfWork) : IOrderService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task<IEnumerable<OrderModel>> GetAllOrdersAsync()
    {
        return await _unitOfWork.Orders.GetAllAsync();
    }

    public async Task<OrderModel?> GetOrderByIdAsync(Guid id)
    {
        return await _unitOfWork.Orders.GetByIdAsync(id);
    }

    public async Task<OrderModel> CreateOrderAsync(OrderModel order)
    {
        var createdOrder = await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        return createdOrder;
    }

    public async Task<bool> UpdateOrderStatusAsync(Guid id, OrderStatusEnum status)
    {
        return await _unitOfWork.Orders.UpdateOrderStatusAsync(id, status);
    }

    public async Task<bool> DeleteOrderAsync(Guid id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order == null) return false;

        if (order.Status == OrderStatusEnum.Finalizado)
        {
            throw new InvalidOperationException("Cannot delete completed orders.");
        }

        var result = await _unitOfWork.Orders.DeleteAsync(id);
        if (result)
        {
            await _unitOfWork.SaveChangesAsync();
        }

        return result;
    }
}