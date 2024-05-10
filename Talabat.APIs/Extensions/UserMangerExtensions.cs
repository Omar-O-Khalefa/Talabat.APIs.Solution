﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extensions
{
	public static class UserMangerExtensions
	{
		public static async Task<AppUser> FindUserWithAdressByEmailAsync( this UserManager<AppUser> userManager ,ClaimsPrincipal User)
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			var user = await userManager.Users.Include(U=> U.Adress).SingleOrDefaultAsync(U => U.Email == email);

			return user;
		}
	}
}