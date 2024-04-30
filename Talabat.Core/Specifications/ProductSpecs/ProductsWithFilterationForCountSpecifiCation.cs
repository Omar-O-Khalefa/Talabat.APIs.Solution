using System.Diagnostics;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
	public class ProductsWithFilterationForCountSpecifiCation : BaseSpecifications<Product>
	{
		public ProductsWithFilterationForCountSpecifiCation(ProductSpecParams specParams)
		: base( p =>
		               (string.IsNullOrEmpty(specParams.Search) || p.Name.ToLower().Contains(specParams.Search)) &&
					   (!specParams.BrandId.HasValue || p.BrandId == specParams.BrandId.Value) &&
					   (!specParams.CategoryId.HasValue || p.CategoryId == specParams.CategoryId.Value))
		{
			
		}
	}
}
