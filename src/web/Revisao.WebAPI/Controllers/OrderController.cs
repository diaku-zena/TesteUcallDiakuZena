using Microsoft.AspNetCore.Mvc;
using Revisao.Application.DTOs;
using Revisao.Application.Services._Order;

namespace Revisao.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var orderId = await _orderService.RegisterOrderAsync(request);
        return CreatedAtAction(nameof(GetOrderStatus), new { id = orderId }, null);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderStatus(Guid id)
    {
        var status = await _orderService.GetOrderStatusAsync(id);
        return Ok(status);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await _orderService.GetAllOrdersAsync();
        return Ok(orders);
    }
}
