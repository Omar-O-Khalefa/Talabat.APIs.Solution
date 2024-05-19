using System.ComponentModel.DataAnnotations.Schema;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod? deliveryMethod, ICollection<OrderItem> items, decimal subtotal,string paymentIntentIdd)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentIdd;
        }

        public string BuyerEmail { get; set; } = null!;
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public Address ShippingAddress { get; set; } = null!;

        //public int DeliveryMethodId { get; set; } // Foreign Key
        public DeliveryMethod? DeliveryMethod { get; set; } = null!;// Navigational Property[ONE]
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>(); // Navigational Property[Many]
        public decimal Subtotal { get; set; }

        //[NotMapped]
        //public decimal Total { get { return Subtotal + DeliveryMethod.Cost; } }  

        public decimal GetTotal() => Subtotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; } 
    }
}
