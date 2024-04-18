namespace Talabat.APIs.Errors
{
	public class APIValidationErrorResponse : APIResponse
	{
		public IEnumerable<string> Errors { get; set; }

		public APIValidationErrorResponse()
		: base(400)
		{
			Errors = new List<string>();
		}

	}
}
