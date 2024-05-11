using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Product;

namespace Talabat.Repository.Data.Config
{
    internal class ProductBrandConfigurations : IEntityTypeConfiguration<ProductBrand>
	{
		public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ProductBrand> builder)
		{

			builder.Property(p => p.Name).IsRequired();

		}
	}
}
