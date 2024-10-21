using SurveyBasket.API.Abstractions;

namespace SurveyBasket.API.Errors
{
	public class UserErrors
	{
		public static readonly Error InvalidCredintials = new("User.InvalidCredintials", "Invalid Email or Password");
	}
}
