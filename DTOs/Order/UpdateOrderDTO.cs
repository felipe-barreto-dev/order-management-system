using System.ComponentModel.DataAnnotations;
using OrderManagementSystem.Enums;

namespace OrderManagementSystem.DTOs.Order;

public class UpdateOrderDTO
{
    [StringLength(100, ErrorMessage = "Customer name must be between 1 and 100 characters")]
    public string? Customer { get; set; }

    [StringLength(100, ErrorMessage = "Product name must be between 1 and 100 characters")]
    public string? Product { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Value must be greater than 0")]
    public decimal? Value { get; set; }

    public OrderStatusEnum? Status { get; set; }
}