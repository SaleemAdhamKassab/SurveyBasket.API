namespace SurveyBasket.API.Contracts.Questions.Requests
{
	public class QuestionRequest
	{
		public string Content { get; set; } = string.Empty;
		public List<string> Answers { get; set; } = [];
	}
}