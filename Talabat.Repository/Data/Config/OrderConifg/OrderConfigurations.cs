using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure.Data.Config.OrderConifg
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(Order => Order.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());
            builder.Property(Order => Order.Status)
                .HasConversion(

                (OStatus) => OStatus.ToString(),
                (OStatus) => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)

                );
            builder.Property(order => order.Subtotal)
                .HasColumnType("decimal(12,2)");
            builder.HasOne(order => order.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
            builder.HasMany(order => order.Items)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

           /// builder.HasOne(order => order.DeliveryMethod)
           ///     .WithOne();
           ///builder.HasIndex("DeliveryMethodId").IsUnique(true);//IF you Wont To Be ONE to One
        }
    }
}
