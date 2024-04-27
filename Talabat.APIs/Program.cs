
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Repository.Data;
using Talabat.Infrastructure;
using Talabat.APIs.DTOs.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Talabat.APIs.Errors;
using Talabat.APIs.Middlewares;
using Talabat.APIs.Extensions;
using StackExchange.Redis;
using Talabat.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Talabat.Core.Entities.Identity;
namespace Talabat.APIs
{
	public class Program
	{
		// Entrypoint
		public static async Task Main(string[] args)
		{
			var WebApplicationbuilder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the container.

			WebApplicationbuilder.Services.AddControllers();
			// Register Required Web APIs Services To the DI Container

			WebApplicationbuilder.Services.AddSwaggerServices();

			WebApplicationbuilder.Services.AddDbContext<StoreContext>(options =>
			options.UseSqlServer(WebApplicationbuilder.Configuration.GetConnectionString("DefaultConnection")));
			
			WebApplicationbuilder.Services.AddDbContext<AppIdentityDbContext>(options =>
			options.UseSqlServer(WebApplicationbuilder.Configuration.GetConnectionString("IdentityDbContext")));

			WebApplicationbuilder.Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
			{
				var connection = WebApplicationbuilder.Configuration.GetConnectionString("Redis");
				return ConnectionMultiplexer.Connect(connection);
			});

			WebApplicationbuilder.Services.AddIdentitySerices();
			WebApplicationbuilder.Services.AddApplicationServices();

			#endregion

			#region Create DataBase For First Time Deploying
			var app = WebApplicationbuilder.Build();

			using var scope = app.Services.CreateScope();

			var services = scope.ServiceProvider;

			var _dbContext = services.GetRequiredService<StoreContext>();
			// Asking Clr For Createing Object From DbCOntext Explicitly
			var loggerFactory = services.GetRequiredService<ILoggerFactory>();
			try
			{
				await _dbContext.Database.MigrateAsync(); // Update-DataBase

				await StoreContextSeed.SeedAsync(_dbContext);

				var IdentityContext = services.GetRequiredService<AppIdentityDbContext>();
				await IdentityContext.Database.MigrateAsync();
				var userManger = services.GetRequiredService<UserManager<AppUser>>();
				await AppIdentityDbContextSead.SeadUsersAsync(userManger);
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "An Error Has been occured During apply the Migrations");
			}

			#endregion

			#region Configure Kestrel Middlewares
			app.UseMiddleware<ExceptionMiddleware>();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwaggerMiddlewares();
			}

			app.UseStatusCodePagesWithReExecute("/errors/{0}");

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.MapControllers(); 
			#endregion

			app.Run();
		}
	}
}
