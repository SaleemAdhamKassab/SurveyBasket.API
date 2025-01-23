using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Contracts.Users.Requests;
using SurveyBasket.API.Contracts.Users.Responses;

namespace SurveyBasket.API.Services.UsersService
{
	public interface IUserService
	{
		Task<IEnumerable<UserResponse>> GetAllAsync();
		Task<Result<UserResponse>> GetAsync(string userId);
		Task<Result<UserResponse>> AddAsync(CreateUserRequest request);
		Task<Result> UpdateAsync(string userId, UpdateUserRequest request);
		Task<Result> ToggleStatusAsync(string userId);
		Task<Result> UnlockAsync(string userId);
		Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
		Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
		Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request);
	}
}
