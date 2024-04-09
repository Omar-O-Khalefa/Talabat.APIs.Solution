
namespace Talabat.APIs
{
	public class Program
	{
		// Entrypoint
		public static void Main(string[] args)
		{
			var WebApplicationbuilder = WebApplication.CreateBuilder(args);

			#region Configure Services
			// Add services to the container.

			WebApplicationbuilder.Services.AddControllers();
			// Register Required Web APIs Services To the DI Container

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			WebApplicationbuilder.Services.AddEndpointsApiExplorer();
			WebApplicationbuilder.Services.AddSwaggerGen(); 
			#endregion

			var app = WebApplicationbuilder.Build();

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
