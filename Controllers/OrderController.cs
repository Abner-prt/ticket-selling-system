using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ticket_selling_backend.DTOs.Orders;
using ticket_selling_backend.Services.Orders;

namespace ticket_selling_backend.Controllers;

[Route("api/orders")]
[ApiController]
[Authorize] // Requires login
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost("checkout")]
    public async Task<ActionResult> Checkout([FromBody] CheckoutRequestDto dto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _orderService.CheckoutAsync(userId, dto);
        return StatusCode(result.StatusCode, result);
    }

    [HttpGet("my-orders")]
    public async Task<ActionResult> GetMyOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _orderService.GetMyOrdersAsync(userId);
        return StatusCode(result.StatusCode, result);
    }
}
