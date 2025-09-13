using Microsoft.EntityFrameworkCore;
using order_management_system.Models;

public class ApplicationDbContext : DbContext
{
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

  public DbSet<OrderModel> Orders { get; set; }
}