using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            ///Must define that address of order class will map at the same table that will map to it Order  
            ///Each Order OwnsOne =>ShippingAddress and ShippingAddress will happens to him Mapping WithOwner => Order
            builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());
            //To store OrderStatus at DB as String and retrieve as OrderStatus[Enum]

            builder.Property(O=>O.Status).HasConversion(oStatus=>oStatus.ToString(),oStatus=>(OrderStatus) Enum.Parse(typeof(OrderStatus),oStatus));
            ///At which code from Two codes it will understand that relationship 1:1
            ///builder.HasOne(O => O.DeliveryMethod)
            /// .WithOne(); //Will make unique constrain on F.K
            ///OR 
            ///builder.HasIndex(O => O.DeliveryMethodId).IsUnique();
            builder.Property(O => O.Subtotal)
                  .HasColumnType("decimal(18,2)");
            builder.HasOne(o => o.DeliveryMethod).WithMany().OnDelete(DeleteBehavior.SetNull);
        }
    }
}
