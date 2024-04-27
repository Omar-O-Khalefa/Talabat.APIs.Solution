using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManger;
		private readonly SignInManager<AppUser> _signInManager;

		public AccountController(UserManager<AppUser> userManger,SignInManager<AppUser> signInManager)
        {
			_userManger = userManger;
			_signInManager = signInManager;
		}

		[HttpPost("Login")] // POST : /api/Account/Login

		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await _userManger.FindByEmailAsync(loginDto.Email);
			if (user == null)
			{
				return Unauthorized(new APIResponse(401));
			}
			var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
			if (!result.Succeeded)
			{
				return Unauthorized(new APIResponse(401));
			}
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = "Thsi Will Be Token"
			});
		}
    }
}
