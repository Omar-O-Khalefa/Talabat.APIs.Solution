
namespace Talabat.APIs.Errors
{
	public class APIResponse
	{
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public APIResponse( int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);

		}

		private string? GetDefaultMessageForStatusCode(int statusCode)
		{
			return statusCode switch
			{
				400 => "Bad Request",
				401 => "Unauthorized",
				404 => "Resource Was Not Found",
				500 => "Error are The path to the Dark",
				_ => null,
			};
		}
	}
}
