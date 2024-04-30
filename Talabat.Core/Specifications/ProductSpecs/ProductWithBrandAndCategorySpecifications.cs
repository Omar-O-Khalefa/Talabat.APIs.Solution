using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {
        // this Constructor Will Be Used For Creating An Object , That Will Be used To get All Products
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams specParams)
            : base(p =>

                       (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) &&
                       (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId.Value) &&
                       (!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId.Value)


            )
        {
            Includes.Add(p => p.brand);
            Includes.Add(p => p.Category);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "priceAsc":
                        //OrderBy = p => p.Price;
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        //OrderByDesc = p => p.Price;
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
            else
            {
                AddOrderBy(p => p.Name);
            }


            ApplyPageinaton(((specParams.PageIndex - 1) * specParams.PageSize), specParams.PageSize);
        }
        // this Constructor Will Be Used For Creating An Object , That Will Be used To get Product By Id
        public ProductWithBrandAndCategorySpecifications(int id)
            : base(p => p.Id == id)
        {
            Includes.Add(p => p.brand);
            Includes.Add(p => p.Category);
        }



    }
}
