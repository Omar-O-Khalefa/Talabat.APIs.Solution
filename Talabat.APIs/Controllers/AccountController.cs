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

		public AccountController(UserManager<AppUser> userManger, SignInManager<AppUser> signInManager)
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

		[HttpPost("Register")] // POST : /api/Account/Register

		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			var user = new AppUser()
			{
				DisplayName = registerDto.DisplayName,
				Email = registerDto.Email,
				PhoneNumber = registerDto.PhoneNumber,
				UserName = registerDto.Email.Split('@')[0]


			};

			var res = await _userManger.CreateAsync(user, registerDto.Password);

			if (!res.Succeeded)
			{
				return BadRequest(new APIResponse(400));
			};

			return Ok(new UserDto()
			{
				DisplayName = registerDto.DisplayName,
				Email = registerDto.Email,
				Token = "ThisWiil Be Token"
			});

		}


	}

}
