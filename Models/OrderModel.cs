using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OrderManagementSystem.Enums;

namespace OrderManagementSystem.Models;

[Table("Orders")]
public class OrderModel
{
  [Key]
  public Guid Id { get; set; } = Guid.NewGuid();

  [Required]
  [StringLength(100)]
  public string Costumer { get; set; } = string.Empty;

  [Required]
  [StringLength(100)]
  public string Product { get; set; } = string.Empty;

  [Required]
  [Range(0.01, double.MaxValue)]
  public decimal Value { get; set; }

  [Required]
  [StringLength(20)]
  public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pendente;

  public DateTime OrderDate { get; set; } = DateTime.UtcNow;
}