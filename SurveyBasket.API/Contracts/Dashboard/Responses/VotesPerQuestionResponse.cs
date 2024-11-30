namespace SurveyBasket.API.Contracts.Dashboard.Responses
{
	public class VotesPerQuestionResponse
	{
		public string Question { get; set; } = string.Empty;
		public IEnumerable<VotesPerAnswerResponse> SelectedAnswers { get; set; } = [];
	}
}
