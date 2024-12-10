using SurveyBasket.API.Models;

namespace SurveyBasket.API.Services.Auth
{
	public interface IJwtProvider
	{
		(string token, int expiresIn) GenerationToken(ApplicationUser user, IEnumerable<string> roles, IEnumerable<string> permissions);
		string? ValidateToken(string token); // return UserId if valid
	}
}
