﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Entities.Product;

namespace Talabat.Repository.Data
{
    public static class StoreContextSeed
	{
		public async static Task SeedAsync(StoreContext _dbContext)
		{
			if (_dbContext.ProductBrands.Count() == 0)
			{
			var brandsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
			var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);

				if (brands?.Count() > 0)
				{
					foreach (var brand in brands)
					{
						_dbContext.Set<ProductBrand>().Add(brand);

					}
					await _dbContext.SaveChangesAsync();
				} 
			}

			if (_dbContext.ProductCategories.Count() == 0)
			{
				var categoriesData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
				var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoriesData);

				if (categories?.Count() > 0)
				{
					foreach (var category in categories)
					{
						_dbContext.Set<ProductCategory>().Add(category);

					}
					await _dbContext.SaveChangesAsync();
				}
			}

			if (_dbContext.Products.Count() == 0)
			{
				var ProductsData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);

				if (products?.Count() > 0)
				{
					foreach (var prod in products)
					{
						_dbContext.Set<Product>().Add(prod);

					}
					await _dbContext.SaveChangesAsync();
				}
			}

			if (_dbContext.DeliveryMethods.Count() == 0)
			{
				var DeliveryMethodData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
				var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryMethodData);

				if (DeliveryMethods?.Count() > 0)
				{
					foreach (var DeliveryMethod in DeliveryMethods)
					{
						_dbContext.Set<DeliveryMethod>().Add(DeliveryMethod);

					}
					await _dbContext.SaveChangesAsync();
				}
			}
		}
	}
}
