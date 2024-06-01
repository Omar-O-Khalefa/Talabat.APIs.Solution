using Microsoft.Extensions.Configuration;
using Stripe;
using Talabat.Core;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpecs;
using ProductM = Talabat.Core.Entities.Product.Product;
namespace Talabat.Service.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(
            IConfiguration configuration,
            IBasketRepository basketRepo,
            IUnitOfWork unitOfWork
            )
        {
            _configuration = configuration;
            _basketRepo = basketRepo;
            _unitOfWork = unitOfWork;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntet(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];

            var basket = await _basketRepo.GetBasketAsync(basketId);

            if (basket is null)
            {
                return null;
            }

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMe = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);

                shippingPrice = deliveryMe.Cost;

                basket.ShippingPrice = shippingPrice;
            }

            if (basket.Items.Count > 0)
            {
                var prodRepo = _unitOfWork.Repository<ProductM>();
                foreach (var item in basket.Items)
                {
                    var Eproduct = await prodRepo.GetByIdAsync(item.Id);

                    if (item.Price != Eproduct.Price)
                    {
                        item.Price = Eproduct.Price;
                    }
                }
            }

            PaymentIntent paymentIntent;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PayemntIntentId)) // Create New Payment Intent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }

                };
                paymentIntent = await paymentIntentService.CreateAsync(options); // Integration with Strip
                basket.PayemntIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update Exisiting Payment Intent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100,
                };
                await paymentIntentService.UpdateAsync(basket.PayemntIntentId, options);
            }
            await _basketRepo.UpdateBasketAsync(basket);
            return basket;
        }

        public async Task<OrderAg?> UpdateOrderStatus(string paymentIntentId, bool Ispaid)
        {
            var orderRepo = _unitOfWork.Repository<OrderAg>();
            var spec = new OrderWithPaymentIntentSpaceification(paymentIntentId);
           var order = await orderRepo.GetByIdWithSpecAsync(spec);

            if(order is null)
            {
                return null;
            }

            if (Ispaid)
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            else
            {
                order.Status = OrderStatus.PaymentReceived;
            }
            orderRepo.Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }
    }
}
