using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        [ProducesResponseType(typeof(CustomerBasket),statusCode:StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse),statusCode:StatusCodes.Status400BadRequest)]
        [HttpPost("{basketid}")] //GET /api/payment/{basket:id}
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketid)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntet(basketid);
            if(basket is null)
            {
                return BadRequest(new APIResponse(404, "An Error With Your Basket"));
            }
            return Ok(basket);
        }

    }
}
