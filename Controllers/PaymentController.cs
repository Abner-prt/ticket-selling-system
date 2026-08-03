using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ticket_selling_backend.Services;

namespace ticket_selling_backend.Controllers;

[Route("api/payments")]
[ApiController]
[Authorize] // Requires login
public class PaymentController : ControllerBase
{
    private readonly StripeService _stripeService;

    public PaymentController(StripeService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("create-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentRequestDto request)
    {
        try
        {
            var paymentIntent = await _stripeService.CreatePaymentIntentAsync(request.Amount);
            return Ok(new { clientSecret = paymentIntent.ClientSecret });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

public class PaymentIntentRequestDto
{
    public long Amount { get; set; }
}
