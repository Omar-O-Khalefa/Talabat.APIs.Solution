using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Infrastructure;
using Talabat.Service.OrderService;
using Talabat.Service.ProductSe;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationServicesExtension
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{

			services.AddScoped(typeof(IProductService), typeof(ProductService));

			services.AddScoped(typeof(IOrderService), typeof(OrderService));

			services.AddScoped(typeof(IUnitOfWork), typeof(UniteOfWork));

			//services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

			services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

			services.AddAutoMapper(typeof(MappingProfiles));

			services.Configure<ApiBehaviorOptions>(Options =>
			{
				Options.InvalidModelStateResponseFactory = (actionContext) =>
				{
					var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count > 0)
														 .SelectMany(p => p.Value.Errors)
														 .Select(E => E.ErrorMessage)
														 .ToList();
					var respons = new APIValidationErrorResponse()
					{
						Errors = errors
					};
					return new BadRequestObjectResult(respons);
				};
			}
			);
			//WebApplicationbuilder.Services.AddAutoMapper(M => M.AddProfile( new MappingProfiles()));
			return services;
		}
	}
}
