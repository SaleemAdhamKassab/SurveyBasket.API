using Microsoft.AspNetCore.Identity;
using SurveyBasket.API.Contracts.Auth;
using SurveyBasket.API.Data;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Services.Auth
{
	public interface IAuthService
	{
		Task<AuthResponse?> GetTokenAsync(string email, string password);
	}

	public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IJwtProvider _jwtProvider = jwtProvider;

		public async Task<AuthResponse?> GetTokenAsync(string email, string password)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user is null)
				return null;

			var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
			if (!isValidPassword)
				return null;

			var (token, expiresIn) = _jwtProvider.GenerationToken(user);
			return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn);
		}
	}
}
