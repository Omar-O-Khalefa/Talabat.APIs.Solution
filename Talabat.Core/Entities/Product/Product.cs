using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Product
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public string PictureUrl { get; set; }

        public decimal Price { get; set; }

        //[ForeignKey(nameof(Product.brand))]
        public int BrandId { get; set; } // Foregin Key Column => ProductBrand

        public ProductBrand brand { get; set; }


        //[ForeignKey(nameof(Product.Category))]
        public int CategoryId { get; set; }
        public ProductCategory Category { get; set; }
    }
}
