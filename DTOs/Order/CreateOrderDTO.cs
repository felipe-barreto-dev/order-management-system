using System.ComponentModel.DataAnnotations;
using OrderManagementSystem.Enums;

namespace OrderManagementSystem.DTOs.Order;

public class CreateOrderDTO
{
    [Required(ErrorMessage = "Customer is required")]
    [StringLength(100, ErrorMessage = "Customer name must be between 1 and 100 characters")]
    public required string Customer { get; set; }

    [Required(ErrorMessage = "Product is required")]
    [StringLength(100, ErrorMessage = "Product name must be between 1 and 100 characters")]
    public required string Product { get; set; }

    [Required(ErrorMessage = "Value is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Value must be greater than 0")]
    public required decimal Value { get; set; }

    // OrderDate e Status não devem ser obrigatórios na criação
    // Eles são definidos automaticamente pelo sistema
    public DateTime? OrderDate { get; set; }
    public OrderStatusEnum? Status { get; set; }
}