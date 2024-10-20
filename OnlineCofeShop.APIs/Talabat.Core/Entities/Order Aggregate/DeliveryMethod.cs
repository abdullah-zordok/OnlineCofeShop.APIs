using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class DeliveryMethod :BaseEntity
    {
        ///Here, Accessible Empty Parameterless Constructor Must be Exist To EFCore   
        ///Constructor to enable EFCore to know use this class(to know Make Migrations)
        public DeliveryMethod()
        {
        }

        public DeliveryMethod(string shortName, string description, decimal cost, string deliveryTime)
        {
            ShortName = shortName;
            Description = description;
            Cost = cost;
            DeliveryTime = deliveryTime;
        }

        public string ShortName { get; set; }
        //It could include information such as the type of service offered 
        public string Description { get; set; }

        public decimal Cost { get; set; }

        //the amount of time it takes for to receive an order to customer(after 2 Days) 
        public string DeliveryTime { get; set; }


        //If you hadn't written this line , EFCore by Default will understand that Naviagational Property as many but idont need it at all
        //public ICollection<Order> Order { get; set; } = new HashSet<Order>(); //Naviagational Property[Many]


    }
}
