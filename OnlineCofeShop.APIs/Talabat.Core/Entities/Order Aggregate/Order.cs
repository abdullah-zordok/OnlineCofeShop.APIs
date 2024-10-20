using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order :BaseEntity
    {
        ///Here, Accessible Empty Parameterless Constructor Must be Exist To EFCore   
        ///Constructor to enable EFCore to know use this class(to know Make Migrations) 
        public Order()
        {
        }

        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItems> items, decimal subtotal, string paymentIntentId)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }


        public string BuyerEmail { get; set; }

        ///What is difference between DateTimeOffset and Datetime
        ///DateTimeOffset: Offset indicate World time difference between Countries(reflects a time's offset from UTC)
        ///To if open App at different Country OrderDate will appears with time this Country
        ///DateTime: the system's local time zone

        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        ///Not exist Navigation Property(Not Mapping address class to Table) ==> Because 
        ///1.ERCore Just only Copy data that exist at Address class and Put it at Order class
        ///2.Relationship between Order and address =>( One To One) and Total from Both sides =>
        ///Make Mapping => Put all Attributes at One Table(Order),Choose PK of any entity to this table

        public Address ShippingAddress { get; set; } //1:1 (T)

        ///Explian Foreign Key Column
        ///EFCore By default Will Make This column at Order Table
        ///=> Because relationShip between Order and DeliveryMethod is [1:M] and Total[M],
        ///So,EFCore Will Take P.K(DeliveryMethodId) from[1](DeliveryMethod) then  Put it as F.K inside [M](Order)
        ///EFCore represent F.K(DeliveryMethodId) as Column inside Order table 

        //public int DeliveryMethodId { get; set; } //Foreign Key[1] 

        public DeliveryMethod DeliveryMethod { get; set; }//Naviagational Property[ONE]

        //Relationship Between OrderItems and Order is [1 To M]
        public ICollection<OrderItems> Items { get; set; } = new HashSet<OrderItems>(); //Naviagational Property[Many]

        ///Subtotal is Total Cost of Order without DeliveryMethodCost
        ///Subtotal +=Price of product * Quantity of products 
        public decimal Subtotal { get; set; }

        //Total = Subtotal + DeliveryMethodCost
        //total is Derived Attribute Which NotMapped to any thing at Database
        //there 2 ways to Make Derived Attribute : 

        //1.
        //[NotMapped]
        //public decimal Total => Subtotal + DeliveryMethod.Cost; 

        //2.

        public decimal GetTotal()
            => Subtotal + DeliveryMethod.Cost;

        ///PaymentIntent: A PaymentIntent represents the intention to collect payment from a customer.
        ///It is a core object in the Stripe API used to handle various aspects of the payment process.
        public string PaymentIntentId { get; set; }
    }
}
