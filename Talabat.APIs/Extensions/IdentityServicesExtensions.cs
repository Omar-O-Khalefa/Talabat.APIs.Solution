using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Infrastructure.Identity;

namespace Talabat.APIs.Extensions
{
	public static class IdentityServicesExtensions
	{
		public static IServiceCollection AddIdentitySerices(this IServiceCollection services,IConfiguration configuration)
		{
			services.AddIdentity<AppUser,IdentityRole>(option =>
			{

			})
				.AddEntityFrameworkStores<AppIdentityDbContext>();


			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidateIssuer = true,
					ValidIssuer = configuration.GetConnectionString("JWT:ValidIssure"),
					ValidateAudience = true,
					ValidAudience = configuration.GetConnectionString("JWT:ValidAudience"),
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWt:Key"]))
				};
			});
			return services;
		}
	}
}
