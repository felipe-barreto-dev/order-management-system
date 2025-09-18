using FluentAssertions;
using Moq;
using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;
using OrderManagementSystem.Repositories.Order;
using OrderManagementSystem.Repositories.UnitOfWork;
using OrderManagementSystem.Services;

namespace OrderManagement.Tests.Services;

public class OrderServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IOrderRepository> _mockOrderRepository;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockOrderRepository = new Mock<IOrderRepository>();
        
        _mockUnitOfWork.Setup(u => u.Orders).Returns(_mockOrderRepository.Object);
        
        _orderService = new OrderService(_mockUnitOfWork.Object);
    }

    [Fact]
    public async Task GetAllOrdersAsync_ShouldReturnAllOrders()
    {
        var expectedOrders = new List<OrderModel>
        {
            new() { Id = Guid.NewGuid(), Costumer = "João", Product = "Produto A", Value = 100 },
            new() { Id = Guid.NewGuid(), Costumer = "Maria", Product = "Produto B", Value = 200 }
        };

        _mockOrderRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(expectedOrders);

        var result = await _orderService.GetAllOrdersAsync();

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedOrders);
        
        _mockOrderRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task CreateOrderAsync_ShouldCreateOrderAndSaveChanges()
    {
        var newOrder = new OrderModel
        {
            Costumer = "Test Customer",
            Product = "Test Product",
            Value = 150.50m
        };

        var createdOrder = new OrderModel
        {
            Id = Guid.NewGuid(),
            Costumer = "Test Customer",
            Product = "Test Product",
            Value = 150.50m,
            Status = OrderStatusEnum.Pendente,
            OrderDate = DateTime.UtcNow
        };

        _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<OrderModel>()))
            .ReturnsAsync(createdOrder);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _orderService.CreateOrderAsync(newOrder);

        result.Should().NotBeNull();
        result.Id.Should().NotBe(Guid.Empty);
        result.Costumer.Should().Be("Test Customer");
        
        _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<OrderModel>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteOrderAsync_WithFinishedOrder_ShouldThrowException()
    {
        var orderId = Guid.NewGuid();
        var finishedOrder = new OrderModel
        {
            Id = orderId,
            Costumer = "João",
            Product = "Produto X",
            Value = 100,
            Status = OrderStatusEnum.Finalizado
        };

        _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync(finishedOrder);

        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => _orderService.DeleteOrderAsync(orderId));

        exception.Message.Should().Be("Cannot delete completed orders.");
        
        _mockOrderRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task DeleteOrderAsync_WithValidOrder_ShouldDeleteAndReturnTrue()
    {
        var orderId = Guid.NewGuid();
        var pendingOrder = new OrderModel
        {
            Id = orderId,
            Costumer = "João",
            Product = "Produto X",
            Value = 100,
            Status = OrderStatusEnum.Pendente
        };

        _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync(pendingOrder);

        _mockOrderRepository.Setup(r => r.DeleteAsync(orderId))
            .ReturnsAsync(true);

        _mockUnitOfWork.Setup(u => u.SaveChangesAsync())
            .ReturnsAsync(1);

        var result = await _orderService.DeleteOrderAsync(orderId);

        result.Should().BeTrue();
        
        _mockOrderRepository.Verify(r => r.GetByIdAsync(orderId), Times.Once);
        _mockOrderRepository.Verify(r => r.DeleteAsync(orderId), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteOrderAsync_WithNonExistentOrder_ShouldReturnFalse()
    {
        var orderId = Guid.NewGuid();

        _mockOrderRepository.Setup(r => r.GetByIdAsync(orderId))
            .ReturnsAsync((OrderModel?)null);

        var result = await _orderService.DeleteOrderAsync(orderId);

        result.Should().BeFalse();
        
        _mockOrderRepository.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateOrderStatusAsync_ShouldCallRepositoryMethod()
    {
        var orderId = Guid.NewGuid();
        var newStatus = OrderStatusEnum.Processando;

        _mockOrderRepository.Setup(r => r.UpdateOrderStatusAsync(orderId, newStatus))
            .ReturnsAsync(true);

        var result = await _orderService.UpdateOrderStatusAsync(orderId, newStatus);

        result.Should().BeTrue();
        
        _mockOrderRepository.Verify(r => r.UpdateOrderStatusAsync(orderId, newStatus), Times.Once);
    }
}