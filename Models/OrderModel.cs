using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using order_management_system.Enums;

namespace order_management_system.Models;

[Table("Orders")]
public class OrderModel
{
  [Key]
  public Guid Id { get; set; } = Guid.NewGuid();

  [Required]
  [StringLength(100)]
  public string Client { get; set; } = string.Empty;

  [Required]
  [StringLength(100)]
  public string Product { get; set; } = string.Empty;

  [Required]
  [Range(0.01, double.MaxValue)]
  public decimal Value { get; set; }

  [Required]
  [StringLength(20)]
  public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pendente;

  public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}