namespace SurveyBasket.API.Contracts.Dashboard.Responses
{
	public class VoteResponse
	{
		public string UserFullName { get; set; } = string.Empty;
		public DateTime Date { get; set; }
		public IEnumerable<QuestionAnswerResponse> SelectedAnswers { get; set; } = [];
	}
}
