
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<OrderAggregate?> CreateOrderAsync(string BuyerEmail,string BasketId,int deliveryMethodId, Entities.Order_Aggregate.OrderAddress shippingAddress);

        Task<IReadOnlyList<OrderAggregate>> GetOrderForUserAsync(string buyerEmail);

        Task<OrderAggregate?> GetOrderByIdForUserAsync(string buyerEmail, int orderId);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
