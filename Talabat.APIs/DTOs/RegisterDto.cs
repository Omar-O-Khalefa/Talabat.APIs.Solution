using System.ComponentModel.DataAnnotations;

namespace Talabat.APIs.DTOs
{
	public class RegisterDto
	{
        [Required]
        public string DisplayName { get; set; } = null!;

		[Required]
        [EmailAddress]
		public string Email { get; set; } = null!;

		//[Required]
		//[Phone]
		//public string PhoneNumber { get; set; } = null!;

		[Required]
		[RegularExpression("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,}$")]
		public string Password { get; set; } = null!;
    }
}
