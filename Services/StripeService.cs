using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace ticket_selling_backend.Services
{
    public class StripeService
    {
        private readonly IConfiguration _configuration;

        public StripeService(IConfiguration configuration)
        {
            _configuration = configuration;
            
            // Inicializar Stripe con la clave secreta desde appsettings.json
            var secretKey = _configuration["Stripe:SecretKey"];
            StripeConfiguration.ApiKey = secretKey;
        }

        public async Task<PaymentIntent> CreatePaymentIntentAsync(long amountInCents, string currency = "usd")
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = amountInCents,
                Currency = currency,
                PaymentMethodTypes = new System.Collections.Generic.List<string>
                {
                    "card",
                },
            };

            var service = new PaymentIntentService();
            return await service.CreateAsync(options);
        }
    }
}
