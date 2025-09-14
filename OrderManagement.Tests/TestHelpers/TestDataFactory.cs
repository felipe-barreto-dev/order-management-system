using OrderManagementSystem.DTOs.Order;
using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;

namespace OrderManagement.Tests.TestHelpers;

/// <summary>
/// Factory para criar dados de teste de forma consistente
/// Centraliza criação de objetos para evitar duplicação nos testes
/// </summary>
public static class TestDataFactory
{
    public static OrderModel CreateValidOrder(string? customer = null, string? product = null, decimal? value = null)
    {
        return new OrderModel
        {
            Id = Guid.NewGuid(),
            Costumer = customer ?? "Test Customer",
            Product = product ?? "Test Product", 
            Value = value ?? 100.00m,
            Status = OrderStatusEnum.Pendente,
            OrderDate = DateTime.UtcNow
        };
    }

    public static List<OrderModel> CreateMultipleOrders(int count = 3)
    {
        var orders = new List<OrderModel>();
        
        for (int i = 1; i <= count; i++)
        {
            orders.Add(new OrderModel
            {
                Id = Guid.NewGuid(),
                Costumer = $"Cliente {i}",
                Product = $"Produto {i}",
                Value = 100.00m * i,
                Status = OrderStatusEnum.Pendente,
                OrderDate = DateTime.UtcNow.AddDays(-i)
            });
        }

        return orders;
    }

    public static CreateOrderDTO CreateValidOrderDTO(string? customer = null, string? product = null, decimal? value = null)
    {
        return new CreateOrderDTO
        {
            Customer = customer ?? "Test Customer",
            Product = product ?? "Test Product",
            Value = value ?? 100.00m
        };
    }

    public static UpdateOrderDTO CreateValidUpdateDTO(string? customer = null, decimal? value = null)
    {
        return new UpdateOrderDTO
        {
            Customer = customer ?? "Updated Customer",
            Value = value ?? 200.00m
        };
    }

    public static OrderResponseDTO CreateOrderResponseDTO(OrderModel order)
    {
        return new OrderResponseDTO
        {
            Id = order.Id,
            Customer = order.Costumer,
            Product = order.Product,
            Value = order.Value,
            Status = order.Status,
            OrderDate = order.OrderDate
        };
    }

    public static OrderListDTO CreateOrderListDTO(OrderModel order)
    {
        return new OrderListDTO
        {
            Id = order.Id,
            Customer = order.Costumer,
            Product = order.Product,
            Value = order.Value
        };
    }
}