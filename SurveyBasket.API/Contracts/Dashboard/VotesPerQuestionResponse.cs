namespace SurveyBasket.API.Contracts.Dashboard
{
	public class VotesPerQuestionResponse
	{
		public string Question { get; set; } = string.Empty;
		public IEnumerable<VotesPerAnswerResponse> SelectedAnswers { get; set; } = [];
	}
}
