namespace SurveyBasket.API.Contracts.Dashboard.Responses
{
	public class VotesPerAnswerResponse
	{
		public string Answer { get; set; } = string.Empty;
		public int Count { get; set; }
	}
}
