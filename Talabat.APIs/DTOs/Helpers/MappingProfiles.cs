using AutoMapper;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.DTOs.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{

			CreateMap<Product, ProductToReturnDto>()
				.ForMember(d => d.brand, o => o.MapFrom(s => s.brand.Name))
				.ForMember(d => d.Category, o => o.MapFrom(s => s.Category.Name))
				//.ForMember(p => p.PictureUrl,o => o.MapFrom(s => $"{_configuration["APIBaseUrl"]}{s.PictureUrl}"));
				.ForMember(p => p.PictureUrl, o => o.MapFrom<ProductPictureUrlResolver>());

			CreateMap<CustomerBasketDto, CustomerBasket>();
			CreateMap<BasketItemDto, BasketItem>();

			CreateMap<AdressDto , Adress>().ReverseMap();
		}
	}
}
