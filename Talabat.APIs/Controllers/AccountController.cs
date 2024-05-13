using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManger;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;

		public AccountController(
			UserManager<AppUser> userManger,
			SignInManager<AppUser> signInManager,
			ITokenService tokenService,
			IMapper mapper
			)
		{
			_userManger = userManger;
			_signInManager = signInManager;
			_tokenService = tokenService;
			_mapper = mapper;
		}

		[HttpPost("login")] // POST : /api/Account/login

		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await _userManger.FindByEmailAsync(loginDto.Email);
			if (user == null)
			{
				return Unauthorized(new APIResponse(401,"Invalid Login"));
			}
			var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
			if (!result.Succeeded)
			{
				return Unauthorized(new APIResponse(401, "Invalid Login"));
			}
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email,
				Token = await _tokenService.CreateToken(user, _userManger)
			});
		}

		[HttpPost("register")] // POST : /api/Account/register

		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			if ( CheckEmailExists(registerDto.Email).Result.Value)
			{
				return BadRequest(new APIValidationErrorResponse()
				{
					Errors = new[] { "This Email Is Already In Use" }
				});
			}
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
				Token = await _tokenService.CreateToken(user, _userManger)
			});

		}

		[Authorize]
		[HttpGet] // GET : /api/Account

		public async Task<ActionResult<UserDto>> GetCurrentUser()
		{
			var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
			var user = await _userManger.FindByEmailAsync(email) ;
			return Ok(new UserDto()
			{
				DisplayName = user.DisplayName,
				Email = user.Email ?? string.Empty,
				Token = await _tokenService.CreateToken(user, _userManger)
			});
		}
		[Authorize]
		[HttpPut("Address")]// PUT : /api/Account/Adress
		public async Task<ActionResult<AddressDto>> UpdateAdress(AddressDto updatedAdress)
		{
			var adress = _mapper.Map<AddressDto, Address>(updatedAdress);


			var appUser = await _userManger.FindUserWithAdressByEmailAsync(User);

			appUser.Address = adress;
			var resault = await _userManger.UpdateAsync(appUser);

			if (!resault.Succeeded)
			{
				return BadRequest(new APIResponse(400, "An Error Occured Dureing Update The Aderss"));
			}
			return Ok(_mapper.Map<Address, AddressDto>(appUser.Address));
		}

		[Authorize]
		[HttpGet("Address")]// GET : /api/Account/Adress

		public async Task<ActionResult<AddressDto>> GetUserAdress()
		{
			var appUser = await _userManger.FindUserWithAdressByEmailAsync(User);
			return Ok(_mapper.Map<Address, AddressDto>(appUser.Address ));
		}


		[HttpGet("EmailExists")]

		public async Task<ActionResult<bool>> CheckEmailExists(string email) 
		{
		return await _userManger.FindByEmailAsync(email)!=null;
		}
	}

}
