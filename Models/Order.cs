using System.ComponentModel.DataAnnotations;
using order_management_system.Enums;

public class Order
{
  [Key]
  public Guid Id { get; set; } = Guid.NewGuid();

  [Required]
  [StringLength(100)]
  public string Cliente { get; set; } = string.Empty;

  [Required]
  [StringLength(100)]
  public string Produto { get; set; } = string.Empty;

  [Required]
  [Range(0.01, double.MaxValue)]
  public decimal Valor { get; set; }

  [Required]
  [StringLength(20)]
  public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pendente;

  public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}