using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities.Product;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.Service.ProductSe
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<Core.Entities.Product.Product>> GetProductsAsync( ProductSpecParams specParams)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(specParams);
            var products = await _unitOfWork.Repository<Core.Entities.Product.Product>().GetAllWithSpecAsync(spec);
            //JsonResult result = new JsonResult(products);

            return products;
        }

        public async Task<Core.Entities.Product.Product?> GetProductAsync(int productId)
        {
            var spec = new ProductWithBrandAndCategorySpecifications(productId);
            var product = await _unitOfWork.Repository<Core.Entities.Product.Product>().GetByIdWithSpecAsync(spec);
            return product;
        }

            public async Task<int> GetCountAsync(ProductSpecParams specParams)
        {
            var CountSpec = new ProductsWithFilterationForCountSpecifiCation(specParams);

            var Count = await _unitOfWork.Repository<Core.Entities.Product.Product>().GetCountAsync(CountSpec);

            return Count;
        }


        public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
        {
            var brands = await _unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return brands;
        }

        public async Task<IReadOnlyList<ProductCategory>> GetCategorysAsync()
        {
            var categories = await _unitOfWork.Repository<ProductCategory>().GetAllAsync();
            return categories;
        }

    
    }
}
