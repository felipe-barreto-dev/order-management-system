using OrderManagementSystem.Enums;

namespace OrderManagementSystem.DTOs.Order
{
  public class CreateOrderDTO
  {
    public required string CustomerId { get; set; }
    public required string ProductId { get; set; }
    public required decimal Value { get; set; }
    public required DateTime OrderDate { get; set; }
    public required OrderStatusEnum Status { get; set; }
  }
}