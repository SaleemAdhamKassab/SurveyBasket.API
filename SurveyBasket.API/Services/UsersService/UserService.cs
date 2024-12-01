using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Abstractions.Result.ApiResult;
using SurveyBasket.API.Contracts.Users.Requests;
using SurveyBasket.API.Contracts.Users.Responses;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Services.UsersService
{
	public class UserService(UserManager<ApplicationUser> userManager) : IUserService
	{
		private readonly UserManager<ApplicationUser> _userManager = userManager;


		public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
		{
			var user = await _userManager.Users
				.Where(e => e.Id == userId)
				.Select(e => new UserProfileResponse
				{
					FirstName = e.FirstName,
					LastName = e.LastName,
					Email = e.Email,
					UserName = e.UserName
				})
				.SingleAsync();

			return Result.Success(user!);
		}
		public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
		{
			//var user = await _userManager.FindByIdAsync(userId);

			//user!.FirstName = request.FirstName;
			//user!.LastName = request.LastName;

			//await _userManager.UpdateAsync(user);

			await _userManager.Users
				.Where(e => e.Id == userId)
				.ExecuteUpdateAsync(prop => prop
									.SetProperty(e => e.FirstName, request.FirstName)
									.SetProperty(e => e.LastName, request.LastName));

			return Result.Success();
		}
		public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
		{
			var user = await _userManager.FindByIdAsync(userId);

			var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

			if (!result.Succeeded)
			{
				var error = result.Errors.First();
				return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
			}

			return Result.Success();
		}
	}
}
