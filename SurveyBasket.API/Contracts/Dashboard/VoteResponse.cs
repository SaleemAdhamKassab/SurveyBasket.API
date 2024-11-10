namespace SurveyBasket.API.Contracts.Dashboard
{
	public class VoteResponse
	{
		public string UserFullName { get; set; } = string.Empty;
		public DateTime Date { get; set; }
		public IEnumerable<QuestionAnswerResponse> SelectedAnswers { get; set; } = [];
	}
}
