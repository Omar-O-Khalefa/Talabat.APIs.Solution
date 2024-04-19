namespace Talabat.APIs.Errors
{
	public class ApiExceptionsResponse : APIResponse
	{
        public string?  Details { get; set; }

        public ApiExceptionsResponse( int ststusCode , string? message =null , string? details = null ) 
            : base(ststusCode, message)
        {
			Details = details;

		}
    }
}
