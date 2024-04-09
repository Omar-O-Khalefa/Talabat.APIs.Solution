using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
	public class Product :BaseEntity
	{
		public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;

		public string PictureUrl { get; set;} = null!;

		public decimal Price { get; set; }

		//[ForeignKey(nameof(Product.brand))]
		public int BrandId { get; set; } // Foregin Key Column => ProductBrand

		public ProductBrand brand { get; set; } = null!;


		//[ForeignKey(nameof(Product.Category))]
		public int CategoryId { get; set; }
        public ProductCategory Category { get; set; } = null!;
    }
}
