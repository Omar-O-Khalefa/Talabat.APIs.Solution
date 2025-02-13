﻿
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Entities.Basket;
using Talabat.Core.Repositories.Contract;

namespace Talabat.Infrastructure
{
    public class BasketRepository : IBasketRepository
	{
		private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer redis)
        {
			_database = redis.GetDatabase();

		}
        public async Task<bool> DeleteBasketAsync(string BasketId)
		{
		return await _database.KeyDeleteAsync(BasketId);	
		}

		public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
		{
			var Basket = await _database.StringGetAsync(BasketId);
			return Basket.IsNullOrEmpty ? null : JsonSerializer.Deserialize<CustomerBasket>(Basket);
		}

		public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket)
		{
			var CreatedOrUpdated = await _database.StringSetAsync(basket.Id,JsonSerializer.Serialize( basket), TimeSpan.FromDays(30));
			if(CreatedOrUpdated is false)
			{
				return null;
			}
			return await GetBasketAsync(basket.Id);
		}
	}
}
