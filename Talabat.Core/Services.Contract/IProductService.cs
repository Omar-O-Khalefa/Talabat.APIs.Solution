﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.Core.Services.Contract
{
    public interface IProductService
    {
        Task<int> GetCountAsync(ProductSpecParams specParams);
        Task<IReadOnlyList<Product?>> GetProductsAsync( ProductSpecParams specParams);
        Task<Product> GetProductAsync(int productId);
        Task<IReadOnlyList<ProductBrand>> GetBrandsAsync();
        Task<IReadOnlyList<ProductCategory>> GetCategorysAsync();
    }
}
