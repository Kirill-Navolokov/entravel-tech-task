using Microsoft.AspNetCore.Mvc;
using OrderProcessingService.Dtos;
using OrderProcessingService.Services;

namespace OrderProcessingService.Controllers;

public class OrderController(IOrderService orderService) : ControllerBase
{
    [HttpPost("order")]
    public async Task<IActionResult> AddAsync([FromBody] RequestOrderDto order)
    {
        var addedOrder = await orderService.AddAsync(order);

        return Ok(addedOrder);
    }

    [HttpGet("order/{id}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id)
    {
        var order = await orderService.GetAsync(id);

        return order is null ? NotFound() : Ok(order);
    }
}