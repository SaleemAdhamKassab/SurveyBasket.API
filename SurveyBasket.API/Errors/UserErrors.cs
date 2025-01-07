using SurveyBasket.API.Abstractions.Result.ApiResult;

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


		public static readonly Error DuplicatedEmail = new("User.DuplicatedEmail",
															   "Another User with the same email is already exists",
																StatusCodes.Status409Conflict);

		public static readonly Error EmailNotConfirmed = new("User.EmailNotConfirmed",
															   "Email is Not Confirmed",
																StatusCodes.Status401Unauthorized);

		public static readonly Error InvalidUserId = new("User.InvalidUserId",
															   "Invalid User Id",
																StatusCodes.Status401Unauthorized);

		public static readonly Error InvalidToken = new("User.InvalidToken",
															   "Invalid Token",
																StatusCodes.Status401Unauthorized);

		public static readonly Error DuplicatedConfirmation = new("User.duplicatedConfirmation",
															   "Email already confirmed",
																StatusCodes.Status400BadRequest);

		public static readonly Error DisabledUser = new("User.DisabledUser",
															   "Disabled User, please contact your Admin",
																StatusCodes.Status400BadRequest);

		public static readonly Error LockedUser = new("User.LockedUser",
															   "Locked User, please contact your Admin",
																StatusCodes.Status400BadRequest);
	}
}