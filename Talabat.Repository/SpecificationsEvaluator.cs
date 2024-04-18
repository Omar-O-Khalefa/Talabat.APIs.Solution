using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Infrastructure
{
	internal class SpecificationsEvaluator <TEntity> where TEntity : BaseEntity
	{
		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> spec)
		{
			var query = inputQuery; // _dbContext.Set<Product>();

			if (spec.Criteria is not null) //  p => p.Id = 1
			{
				query = query.Where(spec.Criteria);
				//_dbContext.Set<Product>().Where(p => p.Id == 1)
			}
				query = spec.Includes.Aggregate(query, (currentQuery, IncludeExpression) => currentQuery.Include(IncludeExpression));
				//_dbContext.Set<Product>().Where(p => p.Id == 1).include(p=>p.brand).include(p=>p.Category)
			return query;
		}
	}
}
