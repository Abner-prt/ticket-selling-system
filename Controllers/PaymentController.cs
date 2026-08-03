using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ticket_selling_backend.Controllers;

[Route("api/payments")]
[ApiController]
public class PaymentController : ControllerBase
{
    [HttpPost("create-intent")]
    public ActionResult CreatePaymentIntent([FromBody] PaymentIntentCreateRequest request)
    {
        try
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = request.Amount,
                Currency = "hnl",
                PaymentMethodTypes = new List<string> { "card" },
            };
            
            var service = new PaymentIntentService();
            var intent = service.Create(options);
            
            return Ok(new { clientSecret = intent.ClientSecret });
        }
        catch (StripeException e)
        {
            return BadRequest(new { message = e.StripeError.Message });
        }
    }
}

public class PaymentIntentCreateRequest
{
    public long Amount { get; set; }
}
