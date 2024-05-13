using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Specifications.OrderSpecs
{
    public class OrderSpecifications :BaseSpecifications<Order>
    {
        public OrderSpecifications( string buyerEmail)
            :base(o => o.BuyerEmail == buyerEmail)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

            AddOrderByDesc(o => o.OrderDate);
        }
        public OrderSpecifications( string buyerEmail,int orderId)
            :base(o => o.BuyerEmail == buyerEmail && o.Id == orderId)
        {
            Includes.Add(o => o.DeliveryMethod);
            Includes.Add(o => o.Items);

       
        }
    }
}
