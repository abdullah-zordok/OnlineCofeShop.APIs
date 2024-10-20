using AutoMapper;
using Talabat.APIs.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles() {
        CreateMap<Product,ProductToReturnDto>().ForMember(d=>d.Brand,O=>O.MapFrom(s=>s.Brand.Name)).ForMember(d=>d.Category,O=>O.MapFrom(s=>s.Category.Name))
                .ForMember(d=>d.PictureUrl,O=>O.MapFrom<ProductPictureUrlResolver>());
            CreateMap<CustomerBasketDto, CustomerBasket>(); 
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Address>();

            CreateMap<AddressDto, Core.Entities.Order_Aggregate.Address>();

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ForMember(ad=>ad.FirstName,d=>d.MapFrom(a=>a.FName)).ForMember(ad => ad.LastName, d => d.MapFrom(a => a.LName)).ReverseMap();

            CreateMap<Order, OrderToReturnDto>()

        .ForMember(d => d.DeliveryMethod, O => O.MapFrom(s => s.DeliveryMethod.ShortName))
        .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItems, OrderItemDto>()
                .ForMember(d => d.ProductName, O => O.MapFrom(s => s.Product.ProductName))
                .ForMember(d => d.ProductId, O => O.MapFrom(s => s.Product.ProductId))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(s => s.Product.PictureUrl))
                .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureUrlResolver>());
         

        }
    }
}
