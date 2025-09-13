using OrderManagementSystem.Enums;

namespace OrderManagementSystem.DTOs.Order;

public class OrderListDTO
{
    public Guid Id { get; set; }
    public string Customer { get; set; } = string.Empty;
    public string Product { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public OrderStatusEnum Status { get; set; }
    public DateTime OrderDate { get; set; }
}