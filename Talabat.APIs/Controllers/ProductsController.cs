using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.APIs.Controllers
{
	public class ProductsController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IMapper _mapper;

		public ProductsController(IGenericRepository<Product> productRepo,IMapper mapper)
        {
			_productRepo = productRepo;
			_mapper = mapper;
		}
		// api/Products
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
		{
			var spec = new ProductWithBrandAndCategorySpecifications();	
			var products = await _productRepo.GetAllWithSpecAsync(spec);
			//JsonResult result = new JsonResult(products);

			return Ok(_mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(products));
		}
		[ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse),StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecifications();
			var product = await _productRepo.GetWithSpecAsync(spec);

			if(product is null)
			{
				return NotFound(new APIResponse(404));
			}
			return Ok(_mapper.Map<Product,ProductToReturnDto>(product));	
		}



    }
}
