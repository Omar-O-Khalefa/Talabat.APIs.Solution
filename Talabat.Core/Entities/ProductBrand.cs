using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
	public class ProductBrand :BaseEntity
	{
		public string Name { get; set; } = null!;
		
		public ICollection<Product> products { get; set; } = new HashSet<Product>();
	}
}
