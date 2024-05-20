using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
   
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        private const string whSecret = "whsec_894099b0db27a129a6089497278e0f7a5dd073c0247b1b1190e24e90c039ea76";

        public PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }
        [Authorize]
        [ProducesResponseType(typeof(CustomerBasket), statusCode: StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), statusCode: StatusCodes.Status400BadRequest)]
        [HttpPost("{basketid}")] //GET /api/payment/{basket:id}
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketid)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntet(basketid);
            if (basket is null)
            {
                return BadRequest(new APIResponse(404, "An Error With Your Basket"));
            }
            return Ok(basket);
        }


        [HttpPost("webhook")]
        public async Task<ActionResult> WebHook()
        {
            OrderAggregate? order;
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json,
                Request.Headers["Stripe-Signature"], whSecret,300,false);
            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            // Handle the event
            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    order= await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);
                    _logger.LogInformation("Order Is Succeeded {0}", order?.PaymentIntentId);
                    _logger.LogInformation("Unhandeld Event Type {0}", stripeEvent?.Type);
                    break;
                case Events.PaymentIntentPaymentFailed:
                    order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
                    _logger.LogInformation("Order Is Not Succeeded {0}", order?.PaymentIntentId);
                    _logger.LogInformation("Unhandeld Event Type {0}", stripeEvent?.Type);
                    break;
            }

            return Ok();

        }
    }
}
