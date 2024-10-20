using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Dtos
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }

        public string BuyerEmail { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public string Status { get; set; }

        public Core.Entities.Order_Aggregate.Address ShippingAddress { get; set; }
        //Deliverymethod Name and Cost 
        public string DeliveryMethod { get; set; }

        public decimal DeliveryMethodCost { get; set; }

        //Map From OrderItem To OrderItemDto 
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();

        public decimal Subtotal { get; set; }

        public decimal Total { get; set; } // Read Value From Method GetTotal() that at (Order)

        public string PaymentIntentId { get; set; } = String.Empty;
    }
}
