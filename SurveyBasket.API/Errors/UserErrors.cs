using SurveyBasket.API.Abstractions;

namespace SurveyBasket.API.Errors
{
	public class UserErrors
	{
		public static readonly Error InvalidCredentials = new("User.InvalidCredintials",
															  "Invalid Email or Password",
															   StatusCodes.Status401Unauthorized);


		public static readonly Error InvalidJwtToken = new("User.InvalidJwtToken",
														   "Invalid Jwt token",
														   StatusCodes.Status401Unauthorized);


		public static readonly Error InvalidRefreshToken = new("User.InvalidRefreshToken",
															   "Invalid refresh token",
															    StatusCodes.Status401Unauthorized);
	}
}
