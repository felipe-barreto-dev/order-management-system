using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystem.DTOs.Order;
using OrderManagementSystem.Models;
using OrderManagementSystem.Services;

namespace OrderManagementSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO order)
        {
            var newOrder = new OrderModel();
            var createdOrder = await _orderService.CreateOrderAsync(newOrder);
            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }
    }
}