
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
        Task<Order?> CreateOrderAsync(string BuyerEmail,string BasketId,int deliveryMethodId, Entities.Order_Aggregate.OrderAddress shippingAddress);

        Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail);

        Task<Order?> GetOrderByIdForUserAsync(string buyerEmail, int orderId);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
