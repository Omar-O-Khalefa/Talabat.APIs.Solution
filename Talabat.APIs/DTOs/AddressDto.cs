﻿using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.DTOs
{
	public class AddressDto
	{
		[Required]
		public string FirstName { get; set; } = null!;
		[Required]
		public string LastName { get; set; } = null!;
		[Required]
		public string Country { get; set; } = null!;
		[Required]
		public string City { get; set; } = null!;
		[Required]
		public string Street { get; set; } = null!;
	

	}
}
