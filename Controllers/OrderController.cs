using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystem.DTOs.Order;
using OrderManagementSystem.Enums;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IOrderService orderService, IMapper mapper) : ControllerBase
{
    private readonly IOrderService _orderService = orderService;
    private readonly IMapper _mapper = mapper;

    /// <summary>
    /// Obtém todos os pedidos
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrderListDTO>>> GetAllOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        var orderDTOs = _mapper.Map<IEnumerable<OrderListDTO>>(orders);
        return Ok(orderDTOs);
    }

    /// <summary>
    /// Obtém um pedido específico por ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<OrderResponseDTO>> GetOrderById(Guid id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        var orderDTO = _mapper.Map<OrderResponseDTO>(order);
        return Ok(orderDTO);
    }

    /// <summary>
    /// Cria um novo pedido
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<OrderResponseDTO>> CreateOrder([FromBody] CreateOrderDTO createOrderDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var orderModel = _mapper.Map<Models.OrderModel>(createOrderDTO);

        var createdOrder = await _orderService.CreateOrderAsync(orderModel);

        var responseDTO = _mapper.Map<OrderResponseDTO>(createdOrder);

        return CreatedAtAction(
            nameof(GetOrderById),
            new { id = createdOrder.Id },
            responseDTO);
    }

    /// <summary>
    /// Atualiza um pedido existente
    /// </summary>
    [HttpPatch("{id}")]
    public async Task<ActionResult<OrderResponseDTO>> UpdateOrder(Guid id, [FromBody] UpdateOrderDTO updateOrderDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var existingOrder = await _orderService.GetOrderByIdAsync(id);
        if (existingOrder == null)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        _mapper.Map(updateOrderDTO, existingOrder);

        var updatedOrder = await _orderService.UpdateOrderAsync(existingOrder);

        var responseDTO = _mapper.Map<OrderResponseDTO>(updatedOrder);
        return Ok(responseDTO);
    }

    /// <summary>
    /// Atualiza o status de um pedido
    /// </summary>
    [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(Guid id, [FromBody] OrderStatusEnum status)
    {
        var success = await _orderService.UpdateOrderStatusAsync(id, status);
        if (!success)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        return NoContent();
    }

    /// <summary>
    /// Exclui um pedido
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        var success = await _orderService.DeleteOrderAsync(id);
        if (!success)
        {
            return NotFound($"Order with ID {id} not found.");
        }

        return NoContent();
    }
}