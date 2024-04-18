﻿using AutoMapper;
using Talabat.Core.Entities;

namespace Talabat.APIs.DTOs.Helpers
{
	public class MappingProfiles : Profile
	{
        public MappingProfiles()
        {
            CreateMap<Product,ProductToReturnDto>()
                .ForMember(d => d.brand,o => o.MapFrom(s => s.brand.Name))
                .ForMember(d => d.Category , o => o .MapFrom(s => s.Category.Name));
        }
    }
}
