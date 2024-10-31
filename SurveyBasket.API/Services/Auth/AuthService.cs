using Microsoft.AspNetCore.Identity;
using SurveyBasket.API.Abstractions;
using SurveyBasket.API.Contracts.Auth;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Models;
using System.Security.Cryptography;

namespace SurveyBasket.API.Services.Auth
{
	public interface IAuthService
	{
		Task<Result<AuthResponse>> GetTokenAsync(string email, string password);
		Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken);
		Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken);
	}

	public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IJwtProvider _jwtProvider = jwtProvider;
		private readonly int _refreshTokenExpireDays = 14;


		private string genereateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));


		public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

			var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
			if (!isValidPassword)
				return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

			var (token, expiresIn) = _jwtProvider.GenerationToken(user);

			var refreshToken = genereateRefreshToken();
			var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpireDays);


			user.RefreshTokens.Add(new RefreshToken
			{
				AddedOn = DateTime.UtcNow,
				ExpiresOn = refreshTokenExpiration,
				Token = refreshToken
			});
			await _userManager.UpdateAsync(user);

			AuthResponse authResponse = new(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);
			return Result.Success(authResponse);
		}

		public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken)
		{
			var userId = _jwtProvider.ValidateToken(token);

			if (userId is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			var user = await _userManager.FindByIdAsync(userId);

			if (user is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

			var userRefreshToken = user.RefreshTokens.SingleOrDefault(e => e.Token == refreshToken && e.IsActive);

			if (userRefreshToken is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

			userRefreshToken.RevokedOn = DateTime.UtcNow;

			var (newToken, expiresIn) = _jwtProvider.GenerationToken(user);

			var newRefreshToken = genereateRefreshToken();
			var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpireDays);


			user.RefreshTokens.Add(new RefreshToken
			{
				AddedOn = DateTime.UtcNow,
				ExpiresOn = refreshTokenExpiration,
				Token = newRefreshToken
			});

			await _userManager.UpdateAsync(user);

			AuthResponse response = new(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpiration);

			return Result.Success(response);
		}

		public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken)
		{
			var userId = _jwtProvider.ValidateToken(token);

			if (userId is null)
				return Result.Failure(UserErrors.InvalidJwtToken);

			var user = await _userManager.FindByIdAsync(userId);

			if (user is null)
				return Result.Failure(UserErrors.InvalidJwtToken);

			var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

			if (userRefreshToken is null)
				return Result.Failure(UserErrors.InvalidRefreshToken);

			userRefreshToken.RevokedOn = DateTime.UtcNow;

			await _userManager.UpdateAsync(user);

			return Result.Success();
		}
	}
}
