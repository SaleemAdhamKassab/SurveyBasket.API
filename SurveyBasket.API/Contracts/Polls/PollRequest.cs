namespace SurveyBasket.API.Contracts.Polls
{
	public class PollRequest
	{
		public string Title { get; set; } = string.Empty;
		public string Summary { get; set; } = string.Empty;
		public DateOnly StartsAt { get; set; }
		public DateOnly EndsAt { get; set; }
	}
}
