

using Talabat.Core;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpecs;

namespace Talabat.Service.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;
        private readonly IGenericRepository<Talabat.Core.Entities.Product.Product> _productRepo;
        private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        private readonly IGenericRepository<Order> _orderRepo;

        public OrderService(
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork,
            IPaymentService paymentService
            //IGenericRepository<Product> productRepo,
            //IGenericRepository<DeliveryMethod> deliveryMethodRepo,
            //IGenericRepository<Order> orderRepo
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //_productRepo = productRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
            //_orderRepo = orderRepo;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int deliveryMethodId, Address shippingAddress)
        {
            // 1.Get Basket From Baskets Repo

            var basket = await _basketRepository.GetBasketAsync(BasketId);

            // 2. Get Selected Items at Basket From Products Repo
            var orderItems = new List<OrderItem>();

            if (basket?.Items.Count > 0)
            {
                var productRepo = _unitOfWork.Repository<Product>();

                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetByIdAsync(item.Id);
                    var productItemOrder = new ProductItemOrder(item.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);

                    orderItems.Add(orderItem);
                }
            }

            // 3. Calculate SubTotal

            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            // 4. Get Delivery Method From DeliveryMethods Repo

            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            var orderRepo = _unitOfWork.Repository<Order>();
            var spec = new OrderWithPaymentIntentSpaceification(basket?.PayemntIntentId);
            var exisitingOrder = await orderRepo.GetByIdWithSpecAsync(spec);

            if (exisitingOrder is not null)
            {
                orderRepo.Delete(exisitingOrder);
                await _paymentService.CreateOrUpdatePaymentIntet(BasketId);
            }

            // 5. Create Order

            var order = new Order
                (
                buyerEmail: BuyerEmail,
                shippingAddress: shippingAddress,
                deliveryMethod: deliveryMethod,
                items: orderItems,
                subtotal: subTotal,
                paymentIntentIdd:basket?.PayemntIntentId ?? ""
                );

            orderRepo.Add(order);

            // 6. Save To Database [TODO]

            var res = await _unitOfWork.CompleteAsync();

            if (res <= 0) return null;

            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId)
        {
            var orderRepo = _unitOfWork.Repository<Order>();
            var orderspec = new OrderSpecifications(buyerEmail, orderId);
            var order = orderRepo.GetByIdWithSpecAsync(orderspec);
            return order;
        }
        //=> _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(new OrderSpecifications(orderId, buyerEmail));
        public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
        {

            var orderRepoo = _unitOfWork.Repository<Order>();
            var spec = new OrderSpecifications(buyerEmail);
            var orders = await orderRepoo.GetAllWithSpecAsync(spec);

            return orders;
        }


    }
}
