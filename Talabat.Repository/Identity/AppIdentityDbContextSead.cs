using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Infrastructure.Identity
{
	public class AppIdentityDbContextSead
	{
		public static async Task SeadUsersAsync(UserManager<AppUser> userManager)
		{
			if (!userManager.Users.Any())
			{
				var user = new AppUser()
				{
					DisplayName = "Omar Khalefa",
					Email = "omarkhalefa235@gmail.com",
					UserName = "Omar.Khalefa",
					PhoneNumber = "01005444826"

				};
				await userManager.CreateAsync(user , "Omar123@#");	
			}
		}
	}
}
