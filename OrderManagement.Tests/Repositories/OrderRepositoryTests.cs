using FluentAssertions;
using OrderManagement.Tests.TestHelpers;
using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Order;

namespace OrderManagement.Tests.Repositories;

public class OrderRepositoryTests : DatabaseTestBase
{
    private readonly OrderRepository _repository;

    public OrderRepositoryTests()
    {
        _repository = new OrderRepository(Context);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOrders()
    {
        var orders = new List<OrderModel>
        {
            new() { Costumer = "João", Product = "Produto A", Value = 100, OrderDate = DateTime.UtcNow.AddDays(-2) },
            new() { Costumer = "Maria", Product = "Produto B", Value = 200, OrderDate = DateTime.UtcNow.AddDays(-1) },
            new() { Costumer = "Pedro", Product = "Produto C", Value = 300, OrderDate = DateTime.UtcNow }
        };

        await SeedDataAsync(orders.ToArray());

        var result = await _repository.GetAllAsync();

        result.Should().NotBeNull();
        result.Should().HaveCount(3);
    }

    [Fact]
    public async Task AddAsync_UsingTestDataFactory_ShouldWork()
    {
        var newOrder = TestDataFactory.CreateValidOrder("Cliente Factory", "Produto Factory", 300.00m);
        
        var createdOrder = await _repository.AddAsync(newOrder);
        await _repository.SaveChangesAsync();

        createdOrder.Should().NotBeNull();
        createdOrder.Costumer.Should().Be("Cliente Factory");
        createdOrder.Product.Should().Be("Produto Factory");
        createdOrder.Value.Should().Be(300.00m);
        
        var fromDb = await Context.Orders.FindAsync(createdOrder.Id);
        fromDb.Should().NotBeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldCreateOrderWithGeneratedId()
    {
        var newOrder = new OrderModel
        {
            Costumer = "Test Customer",
            Product = "Test Product",
            Value = 150.50m,
            Status = OrderStatusEnum.Pendente
        };

        var createdOrder = await _repository.AddAsync(newOrder);
        await _repository.SaveChangesAsync();

        createdOrder.Should().NotBeNull();
        createdOrder.Id.Should().NotBe(Guid.Empty);
        createdOrder.Costumer.Should().Be("Test Customer");

        var fromDb = await Context.Orders.FindAsync(createdOrder.Id);
        fromDb.Should().NotBeNull();
        fromDb!.Value.Should().Be(150.50m);
    }

    [Fact]
    public async Task UpdateOrderStatusAsync_ShouldUpdateStatusSuccessfully()
    {
        var order = new OrderModel
        {
            Costumer = "João",
            Product = "Produto X",
            Value = 100,
            Status = OrderStatusEnum.Pendente
        };

        await SeedDataAsync(order);

        var result = await _repository.UpdateOrderStatusAsync(order.Id, OrderStatusEnum.Processando);

        result.Should().BeTrue();

        var updatedOrder = await Context.Orders.FindAsync(order.Id);
        updatedOrder!.Status.Should().Be(OrderStatusEnum.Processando);
    }

    [Fact]
    public async Task UpdateOrderStatusAsync_WithInvalidId_ShouldReturnFalse()
    {
        var nonExistentId = Guid.NewGuid();

        var result = await _repository.UpdateOrderStatusAsync(nonExistentId, OrderStatusEnum.Finalizado);

        result.Should().BeFalse();
    }
}