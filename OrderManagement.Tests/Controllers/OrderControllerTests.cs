using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using OrderManagementSystem.Controllers;
using OrderManagementSystem.DTOs.Order;
using OrderManagementSystem.Enums;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;

namespace OrderManagement.Tests.Controllers;

public class OrderControllerTests
{
    private readonly Mock<IOrderService> _mockOrderService;
    private readonly Mock<IMapper> _mockMapper;
    private readonly OrderController _controller;

    public OrderControllerTests()
    {
        _mockOrderService = new Mock<IOrderService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new OrderController(_mockOrderService.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllOrders_ShouldReturnOkWithOrderListDTOs()
    {
        var orders = new List<OrderModel>
        {
            new() { Id = Guid.NewGuid(), Costumer = "João", Product = "Produto A", Value = 100 },
            new() { Id = Guid.NewGuid(), Costumer = "Maria", Product = "Produto B", Value = 200 }
        };

        var orderDTOs = new List<OrderListDTO>
        {
            new() { Id = orders[0].Id, Customer = "João", Product = "Produto A", Value = 100 },
            new() { Id = orders[1].Id, Customer = "Maria", Product = "Produto B", Value = 200 }
        };

        _mockOrderService.Setup(s => s.GetAllOrdersAsync())
            .ReturnsAsync(orders);

        _mockMapper.Setup(m => m.Map<IEnumerable<OrderListDTO>>(orders))
            .Returns(orderDTOs);

        var result = await _controller.GetAllOrders();

        result.Should().BeOfType<ActionResult<IEnumerable<OrderListDTO>>>();
        
        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        
        var returnedDTOs = okResult.Value as IEnumerable<OrderListDTO>;
        returnedDTOs.Should().BeEquivalentTo(orderDTOs);
    }

    [Fact]
    public async Task GetOrderById_WithValidId_ShouldReturnOkWithOrderResponseDTO()
    {
        var orderId = Guid.NewGuid();
        var order = new OrderModel 
        { 
            Id = orderId, 
            Costumer = "João", 
            Product = "Produto X", 
            Value = 150.50m 
        };

        var orderDTO = new OrderResponseDTO 
        { 
            Id = orderId, 
            Customer = "João", 
            Product = "Produto X", 
            Value = 150.50m 
        };

        _mockOrderService.Setup(s => s.GetOrderByIdAsync(orderId))
            .ReturnsAsync(order);

        _mockMapper.Setup(m => m.Map<OrderResponseDTO>(order))
            .Returns(orderDTO);

        var result = await _controller.GetOrderById(orderId);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        
        var returnedDTO = okResult.Value as OrderResponseDTO;
        returnedDTO.Should().BeEquivalentTo(orderDTO);
    }

    [Fact]
    public async Task GetOrderById_WithInvalidId_ShouldReturnNotFound()
    {
        var orderId = Guid.NewGuid();

        _mockOrderService.Setup(s => s.GetOrderByIdAsync(orderId))
            .ReturnsAsync((OrderModel?)null);

        var result = await _controller.GetOrderById(orderId);

        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
        notFoundResult.Value.Should().Be($"Order with ID {orderId} not found.");
    }

    [Fact]
    public async Task CreateOrder_WithValidDTO_ShouldReturnCreatedAtAction()
    {
        var createOrderDTO = new CreateOrderDTO
        {
            Customer = "Test Customer",
            Product = "Test Product",
            Value = 100.00m
        };

        var orderModel = new OrderModel
        {
            Costumer = "Test Customer",
            Product = "Test Product",
            Value = 100.00m,
            Status = OrderStatusEnum.Pendente
        };

        var createdOrder = new OrderModel
        {
            Id = Guid.NewGuid(),
            Costumer = "Test Customer",
            Product = "Test Product",
            Value = 100.00m,
            Status = OrderStatusEnum.Pendente,
            OrderDate = DateTime.UtcNow
        };

        var responseDTO = new OrderResponseDTO
        {
            Id = createdOrder.Id,
            Customer = "Test Customer",
            Product = "Test Product",
            Value = 100.00m,
            Status = OrderStatusEnum.Pendente,
            OrderDate = createdOrder.OrderDate
        };

        _mockMapper.Setup(m => m.Map<OrderModel>(createOrderDTO))
            .Returns(orderModel);

        _mockOrderService.Setup(s => s.CreateOrderAsync(orderModel))
            .ReturnsAsync(createdOrder);

        _mockMapper.Setup(m => m.Map<OrderResponseDTO>(createdOrder))
            .Returns(responseDTO);

        var result = await _controller.CreateOrder(createOrderDTO);

        var createdAtActionResult = result.Result as CreatedAtActionResult;
        createdAtActionResult.Should().NotBeNull();
        createdAtActionResult!.StatusCode.Should().Be(201);
        createdAtActionResult.ActionName.Should().Be("GetOrderById");
        
        var routeValues = createdAtActionResult.RouteValues;
        routeValues.Should().ContainKey("id");
        routeValues!["id"].Should().Be(createdOrder.Id);
        
        var returnedDTO = createdAtActionResult.Value as OrderResponseDTO;
        returnedDTO.Should().BeEquivalentTo(responseDTO);
    }

    [Fact]
    public async Task CreateOrder_WithInvalidModelState_ShouldReturnBadRequest()
    {
        var createOrderDTO = new CreateOrderDTO
        {
            Customer = "",
            Product = "Test Product",
            Value = 100.00m
        };

        _controller.ModelState.AddModelError("Customer", "Customer is required");

        var result = await _controller.CreateOrder(createOrderDTO);

        var badRequestResult = result.Result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult!.StatusCode.Should().Be(400);
        
        _mockOrderService.Verify(s => s.CreateOrderAsync(It.IsAny<OrderModel>()), Times.Never);
    }

    [Fact]
    public async Task UpdateOrder_WithValidData_ShouldReturnOkWithUpdatedOrder()
    {
        var orderId = Guid.NewGuid();
        var updateOrderDTO = new UpdateOrderDTO
        {
            Customer = "Updated Customer",
            Value = 250.00m
        };

        var existingOrder = new OrderModel
        {
            Id = orderId,
            Costumer = "Original Customer",
            Product = "Original Product",
            Value = 100.00m,
            Status = OrderStatusEnum.Pendente
        };

        var updatedOrder = new OrderModel
        {
            Id = orderId,
            Costumer = "Updated Customer",
            Product = "Original Product",
            Value = 250.00m,
            Status = OrderStatusEnum.Pendente
        };

        var responseDTO = new OrderResponseDTO
        {
            Id = orderId,
            Customer = "Updated Customer",
            Product = "Original Product",
            Value = 250.00m,
            Status = OrderStatusEnum.Pendente
        };

        _mockOrderService.Setup(s => s.GetOrderByIdAsync(orderId))
            .ReturnsAsync(existingOrder);

        _mockMapper.Setup(m => m.Map(updateOrderDTO, existingOrder))
            .Returns(updatedOrder);

        _mockOrderService.Setup(s => s.UpdateOrderAsync(existingOrder))
            .ReturnsAsync(updatedOrder);

        _mockMapper.Setup(m => m.Map<OrderResponseDTO>(updatedOrder))
            .Returns(responseDTO);

        var result = await _controller.UpdateOrder(orderId, updateOrderDTO);

        var okResult = result.Result as OkObjectResult;
        okResult.Should().NotBeNull();
        okResult!.StatusCode.Should().Be(200);
        
        var returnedDTO = okResult.Value as OrderResponseDTO;
        returnedDTO.Should().BeEquivalentTo(responseDTO);
    }

    [Fact]
    public async Task UpdateOrder_WithNonExistentId_ShouldReturnNotFound()
    {
        var orderId = Guid.NewGuid();
        var updateOrderDTO = new UpdateOrderDTO
        {
            Customer = "Updated Customer"
        };

        _mockOrderService.Setup(s => s.GetOrderByIdAsync(orderId))
            .ReturnsAsync((OrderModel?)null);

        var result = await _controller.UpdateOrder(orderId, updateOrderDTO);

        var notFoundResult = result.Result as NotFoundObjectResult;
        notFoundResult.Should().NotBeNull();
        notFoundResult!.StatusCode.Should().Be(404);
        notFoundResult.Value.Should().Be($"Order with ID {orderId} not found.");
    }
}