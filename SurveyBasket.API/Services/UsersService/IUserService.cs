using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Contracts.Users.Requests;
using SurveyBasket.API.Contracts.Users.Responses;

namespace SurveyBasket.API.Services.UsersService
{
	public interface IUserService
	{
		Task<Result<UserProfileResponse>> GetProfileAsync(string userId);
		Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request);
	}
}
