using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<AddressDto,OrderAddress>();

            CreateMap<Product, ProductToReturnDto>()
               .ForMember(d => d.brand, o => o.MapFrom(s => s.brand.Name))
               .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
               //.ForMember(p => p.PictureUrl,o => o.MapFrom(s => $"{_configuration["APIBaseUrl"]}{s.PictureUrl}"));
               .ForMember(p => p.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.brand, o => o.MapFrom(s => s.brand.Name))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
                //.ForMember(p => p.PictureUrl,o => o.MapFrom(s => $"{_configuration["APIBaseUrl"]}{s.PictureUrl}"));
                .ForMember(p => p.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());

            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<AddressDto, OrderAddress>().ReverseMap();
            CreateMap<AddressDto, identityAddress>().ReverseMap();

            CreateMap<OrderAg, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, o => o.MapFrom(s => s.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, o => o.MapFrom(s => s.DeliveryMethod.Cost));

            CreateMap<OrderItem, OrderItemDto>()
            .ForMember(d => d.ProductId, o => o.MapFrom(s => s.Product.ProductId))
            .ForMember(d => d.ProductName, o => o.MapFrom(s => s.Product.ProductName))
            .ForMember(d => d.PictureUrl, o => o.MapFrom(s => s.Product.PictureUrl))
            .ForMember(d => d.PictureUrl, o => o.MapFrom<OrderItemPictureUrlResolver>());





        }
    }
}
