using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Tests.TestHelpers;
using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;

namespace OrderManagement.Tests.UnitOfWork;

public class UnitOfWorkTests : DatabaseTestBase
{
    private readonly OrderManagementSystem.Repositories.UnitOfWork.UnitOfWork _unitOfWork;

    public UnitOfWorkTests()
    {
        _unitOfWork = new OrderManagementSystem.Repositories.UnitOfWork.UnitOfWork(Context);
    }

    [Fact]
    public async Task SaveChanges_ShouldPersistMultipleOrders()
    {
        var orders = TestDataFactory.CreateMultipleOrders(2);
        var order1 = orders[0];
        var order2 = orders[1];

        await _unitOfWork.Orders.AddAsync(order1);
        await _unitOfWork.Orders.AddAsync(order2);
        var result = await _unitOfWork.SaveChangesAsync();

        result.Should().Be(2); // 2 registros salvos
        
        var ordersInDb = await Context.Orders.ToListAsync();
        ordersInDb.Should().HaveCount(2);
        ordersInDb.Should().Contain(o => o.Costumer == order1.Costumer);
        ordersInDb.Should().Contain(o => o.Costumer == order2.Costumer);
    }

    [Fact]
    public async Task SaveChanges_WithNoChanges_ShouldReturnZero()
    {
        var result = await _unitOfWork.SaveChangesAsync();

        result.Should().Be(0);
    }

    [Fact] 
    public async Task SaveChangesAsync_WithCancellationToken_ShouldWork()
    {
        var order = new OrderModel
        {
            Costumer = "Teste Token",
            Product = "Produto Token",
            Value = 100.00m,
            Status = OrderStatusEnum.Pendente
        };

        await _unitOfWork.Orders.AddAsync(order);

        var result = await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        result.Should().Be(1);
    }

    [Fact]
    public void Orders_Property_ShouldReturnSameInstanceOnMultipleCalls()
    {
        var orders1 = _unitOfWork.Orders;
        var orders2 = _unitOfWork.Orders;

        orders1.Should().BeSameAs(orders2);
        orders1.Should().NotBeNull();
    }

    [Fact]
    public void HasActiveTransaction_Initially_ShouldBeFalse()
    {
        _unitOfWork.HasActiveTransaction.Should().BeFalse();
    }

    [Fact]
    public async Task UnitOfWork_ShouldCoordinateRepositoryOperations()
    {
        var orderId = Guid.NewGuid();
        var order = new OrderModel
        {
            Id = orderId,
            Costumer = "João Coordenação",
            Product = "Produto Coordenação",
            Value = 300.00m,
            Status = OrderStatusEnum.Pendente
        };

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.SaveChangesAsync();

        var savedOrder = await _unitOfWork.Orders.GetByIdAsync(orderId);
        await _unitOfWork.Orders.UpdateOrderStatusAsync(orderId, OrderStatusEnum.Processando);

        savedOrder.Should().NotBeNull();
        savedOrder!.Costumer.Should().Be("João Coordenação");
        
        var updatedOrder = await _unitOfWork.Orders.GetByIdAsync(orderId);
        updatedOrder!.Status.Should().Be(OrderStatusEnum.Processando);
    }

    public override void Dispose()
    {
        _unitOfWork.Dispose();
        base.Dispose(); // Chama o Dispose da DatabaseTestBase
    }
}