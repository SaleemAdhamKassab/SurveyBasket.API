using SurveyBasket.API.Abstractions;

namespace SurveyBasket.API.Errors
{
	public class PollErrors
	{
		public static readonly Error PollNotFound = new("Poll.NotFound", "No poll found with the given ID");
	}
}
