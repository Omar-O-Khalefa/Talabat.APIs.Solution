
using Microsoft.EntityFrameworkCore;
using Talabat.Repository.Data;

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

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			WebApplicationbuilder.Services.AddEndpointsApiExplorer();
			WebApplicationbuilder.Services.AddSwaggerGen();

			WebApplicationbuilder.Services.AddDbContext<StoreContext>(options =>
			options.UseSqlServer(WebApplicationbuilder.Configuration.GetConnectionString("DefaultConnection"))
			);
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
			}
			catch (Exception ex)
			{
				var logger = loggerFactory.CreateLogger<Program>();
				logger.LogError(ex, "An Error Has been occured During apply the Migrations");
			}

			#endregion

			#region Configure Kestrel Middlewares
			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();


			app.MapControllers(); 
			#endregion

			app.Run();
		}
	}
}
