
using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middlewares
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IWebHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next , ILogger<ExceptionMiddleware> logger,IWebHostEnvironment env )
        {
			_next = next;
			_logger = logger;
			_env = env;
		}
        public async Task InvokeAsync(HttpContext httpContext) 
		{
			try
			{
				// Take Action With Resopne
				await _next.Invoke(httpContext);
				// Take Action With Response
			}
			catch (Exception ex)
			{

				_logger.LogError(ex.Message);
				// LOg Excption In Datat Base | File

				httpContext.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
				httpContext.Response.ContentType = "application/json";

				var respons = _env.IsDevelopment() ?
									new ApiExceptionsResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
									:
								    new ApiExceptionsResponse((int)HttpStatusCode.InternalServerError);

				var optios = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
				var json = JsonSerializer.Serialize(respons);

				httpContext.Response.WriteAsync(json);

			}
		}
	}
}
