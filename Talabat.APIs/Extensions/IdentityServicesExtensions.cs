using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
using Talabat.Infrastructure.Identity;

namespace Talabat.APIs.Extensions
{
	public static class IdentityServicesExtensions
	{
		public static IServiceCollection AddIdentitySerices(this IServiceCollection services)
		{
			services.AddIdentity<AppUser,IdentityRole>(option =>
			{

			})
				.AddEntityFrameworkStores<AppIdentityDbContext>();
			services.AddAuthentication();

			return services;
		}
	}
}
