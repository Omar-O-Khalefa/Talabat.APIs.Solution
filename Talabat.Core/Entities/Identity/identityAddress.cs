﻿namespace Talabat.Core.Entities.Identity
{
	public class identityAddress

	{
        public int Id { get; set; }

		public string FirstName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public string Country { get; set; } = null!;
		public string City { get; set; } = null!;
		public string Street { get; set; } = null!;
		public string AppUserId { get; set; } = null!;
        public  AppUser User { get; set; } = null!; // Navigational Peroperty [One] 
	}
}