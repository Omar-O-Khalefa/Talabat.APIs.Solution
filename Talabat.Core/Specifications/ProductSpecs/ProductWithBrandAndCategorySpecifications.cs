using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecs
{
	public class ProductWithBrandAndCategorySpecifications :BaseSpecifications<Product>
	{
        // this Constructor Will Be Used For Creating An Object , That Will Be used To get All Products
        public ProductWithBrandAndCategorySpecifications()
            : base()
		{
			Includes.Add(p => p.brand);
			Includes.Add(p => p.Category);
		}
		// this Constructor Will Be Used For Creating An Object , That Will Be used To get Product By Id
		public ProductWithBrandAndCategorySpecifications( int id)
            : base(p=>p.Id == id)
        {
			Includes.Add(p => p.brand);
			Includes.Add(p => p.Category);
		}
	


    }
}
