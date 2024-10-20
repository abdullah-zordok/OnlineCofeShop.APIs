namespace Talabat.Core.Entities.Order_Aggregate
{///Not Mapped to Table at DB => because RelationShip between ProductItemOrdered and OrderItems 
 ///is (1 To 1) and Total from 2 Sides because one orderitem contain on one product 
 ///So,put all attributes for Two classes  at One Table(OrderItems)
 ///and Choose PK of any entity to this table.

    public class ProductItemOrdered :BaseEntity
    {
        ///Here, Accessible Empty Parameterless Constructor Must be Exist To EFCore   
        ///Constructor to enable EFCore to know use this class(to know Make Migrations)
        public ProductItemOrdered()
        {
        }

        public ProductItemOrdered(int productId, string? productName, string? pictureUrl)
        {
            ProductId = productId;
            ProductName = productName;
            PictureUrl = pictureUrl;
        }

        public int ProductId { get; set; }

        public string? ProductName { get; set; }

        public string? PictureUrl { get; set; }
    }
}