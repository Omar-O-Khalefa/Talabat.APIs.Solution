using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities.Product;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    {

        //private readonly IGenericRepository<Product> _productRepo;
        //private readonly IGenericRepository<ProductBrand> _brandsRepo;
        //private readonly IGenericRepository<ProductCategory> _categoryRepo;

        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(

            //IGenericRepository<Product> productRepo,
            //IGenericRepository<ProductBrand> brandsRepo,
            //IGenericRepository<ProductCategory> CategoryRepo,

            IProductService productService,
            IMapper mapper)
        {
            //_productRepo = productRepo;
            //_brandsRepo = brandsRepo;
            //_categoryRepo = CategoryRepo;

            _productService = productService;
            _mapper = mapper;
        }
        // api/Products
        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public async Task<ActionResult<Pageination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specParams)
        {

            var products = await _productService.GetProductsAsync(specParams);
            //JsonResult result = new JsonResult(products);

            var Data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var Count = await _productService.GetCountAsync(specParams);

            return Ok(new Pageination<ProductToReturnDto>(specParams.PageIndex, specParams.PageSize, Count, Data));
        }

        [ProducesResponseType(typeof(ProductToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await _productService.GetProductAsync(id);

            if (product is null)
            {
                return NotFound(new APIResponse(404));
            }
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));
        }
        [HttpGet("brands")] // Get : Api/Producs/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _productService.GetBrandsAsync();
            return Ok(brands);
        }

        [HttpGet("categories")] // Get : Api/Producs/category
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var categories = await _productService.GetCategorysAsync();
            return Ok(categories);
        }


    }
}
