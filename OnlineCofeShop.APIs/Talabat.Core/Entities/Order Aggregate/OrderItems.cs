namespace Talabat.Core.Entities.Order_Aggregate
{
    public class OrderItems :BaseEntity
    {
        ///Table at DB 
        ///OrderItem : Product that you select it as item inside Order 
        ///OrderItem contain Data about Product
        ///RelationShip between Order and OrderItems => Order contain Many OrderItems(1 To M )


        ///Here, Accessible Empty Parameterless Constructor Must be Exist To EFCore   
        ///Constructor to enable EFCore to know use this class(to know Make Migrations)
        public OrderItems()
        {
        }

        public OrderItems(ProductItemOrdered product, decimal price, int quantity)
        {
            Product = product;
            Price = price;
            Quantity = quantity;
        }


        ///What is difference between Id of orderitem and ProductId
        ///Id => that OrderItem inherited it from (BaseEntity),
        ///this is Id for Orderitem Stored at record of table Orderitems(all order item has different id) 
        ///ProductId => Id of Product and Stored at record of table Product  


        ///From Rules writing Clean Code => if there properies represent particular thing =>
        ///Enable encapsulate this properies at particular type
        ///three Properties represent about (product) that will order it as item inside order
        ///So,Will express about This Properties at one property of particular type 

        public ProductItemOrdered Product { get; set; }

        ///What is difference between Price of orderitem and Price of Product
        ///Price at table OrderItems => product price as a item inside Order
        ///Price at table Product    => product price is constant at system
        public decimal Price { get; set; }

        public int Quantity { get; set; } //Number of units to product as a item inside Order
    }
}