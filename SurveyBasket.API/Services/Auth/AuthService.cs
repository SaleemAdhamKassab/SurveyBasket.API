using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Abstractions.Result.ApiResult;
using SurveyBasket.API.Contracts.Auth.Requests;
using SurveyBasket.API.Contracts.Auth.Responses;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Helpers;
using SurveyBasket.API.Models;
using System.Security.Cryptography;
using System.Text;

namespace SurveyBasket.API.Services.Auth
{
	public class AuthService(
		UserManager<ApplicationUser> userManager,
		SignInManager<ApplicationUser> signInManager,
		IJwtProvider jwtProvider,
		IEmailSender emailSender,
		IHttpContextAccessor httpContextAccessor) : IAuthService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
		private readonly IJwtProvider _jwtProvider = jwtProvider;
		private readonly IEmailSender _emailSender = emailSender;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		private readonly int _refreshTokenExpireDays = 14;


		private string genereateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
		private async Task<AuthResponse> getAuthResponse(ApplicationUser user)
		{
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

			var authResponse = new AuthResponse
			{
				Id = user.Id,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Token = token,
				ExpiresIn = expiresIn,
				RefreshToken = refreshToken,
				RefreshTokenExpiration = refreshTokenExpiration
			};

			return authResponse;
		}
		private async Task sendConfirmationEmail(ApplicationUser user, string token)
		{
			var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

			// send email
			var emailTemplateModel = new Dictionary<string, string>()
			{
				{
					"{{name}}",user.FirstName
				},
				{
					"{{action_url}}",$"{origin}/auth/emailConfirmation?userId={user.Id}&token={token}"
				}
			};

			var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation", emailTemplateModel);
			await _emailSender.SendEmailAsync(user.Email, "Survey Basket: Email Confirmation", emailBody);

		}



		public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password)
		{
			var user = await _userManager.FindByEmailAsync(email);

			if (user is null)
				return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

			//var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
			//if (!isValidPassword)
			//	return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

			var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

			if (!result.Succeeded)
			{
				// check if email is confirmed or not
				return Result.Failure<AuthResponse>(
					result.IsNotAllowed
					? UserErrors.EmailNotConfirmed
					: UserErrors.InvalidCredentials);
			}

			var authResponse = await getAuthResponse(user);
			return Result.Success(authResponse);
		}
		public async Task<Result> RegisterAsync(RegisterRequest request)
		{
			var emailIsExists = await _userManager.Users.AnyAsync(e => e.Email == request.Email);

			if (emailIsExists)
				return Result.Failure<AuthResponse>(UserErrors.DuplicatedEmail);

			var user = new ApplicationUser
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				Email = request.Email,
				UserName = request.Email
			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded)
			{
				var error = result.Errors.First();
				return Result.Failure<AuthResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
			}

			//1) Register local
			//var authResponse = await getAuthResponse(user);

			//2) Register by email
			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
			await sendConfirmationEmail(user, token);
			return Result.Success();
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

			var response = new AuthResponse
			{
				Id = user.Id,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Token = newToken,
				ExpiresIn = expiresIn,
				RefreshToken = newRefreshToken,
				RefreshTokenExpiration = refreshTokenExpiration
			};

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
		public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
		{
			var user = await _userManager.FindByIdAsync(request.UserId);

			if (user is null)
				//return Result.Failure(UserErrors.InvalidUserId); //Not Recommended
				return Result.Failure(UserErrors.InvalidToken);

			if (user.EmailConfirmed)
				return Result.Failure(UserErrors.DuplicatedConfirmation);

			var token = request.Token;

			try
			{
				token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
			}
			catch (FormatException)
			{
				return Result.Failure(UserErrors.InvalidToken);
			}

			var result = await _userManager.ConfirmEmailAsync(user, token);

			if (!result.Succeeded)
			{
				var error = result.Errors.First();
				return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
			}

			return Result.Success();
		}

		public async Task<Result> ResendConfirmationEmail(ResendConfirmationEmailRequest request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email);

			if (user is null)
				return Result.Success(); //To mislead

			if (user.EmailConfirmed)
				return Result.Failure(UserErrors.DuplicatedConfirmation);

			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
			await sendConfirmationEmail(user, token);

			return Result.Success();
		}
	}
}






