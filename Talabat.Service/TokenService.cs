using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Service
{
	public class TokenService : ITokenService
	{
		private IConfiguration _configuration;

		public TokenService(IConfiguration configuration)
        {
			_configuration = configuration;
			
		}

		public IConfiguration Configuration { get; }

		public async Task<string> CreateToken(AppUser user, UserManager<AppUser> userManager)
		{
			//Private Clamis (User-Defined)
		var authClaims = new List<Claim>() 
		{
			new Claim (ClaimTypes.Email, user.Email),
			new Claim (ClaimTypes.GivenName , user.DisplayName)
		};
			var userRole = await userManager.GetRolesAsync(user);
			foreach(var role in userRole)
			{
				authClaims.Add(new Claim(ClaimTypes.Role, role));	
			}
			//Secret Key
			var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWt:Key"]));

			var token = new JwtSecurityToken
				(
				issuer: _configuration["JWT:ValidIssure"],
				audience: _configuration["JWT:ValidAudience"],
				expires:DateTime.Now.AddDays( double.Parse(_configuration["JWt:DurationInDays"])),
				claims: authClaims,
				signingCredentials: new SigningCredentials(authKey,SecurityAlgorithms.HmacSha256)

				);
			return new JwtSecurityTokenHandler().WriteToken(token);
			
		}
	}
}
 