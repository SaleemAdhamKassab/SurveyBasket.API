using SurveyBasket.API.Contracts.Answers;

namespace SurveyBasket.API.Contracts.Questions
{
	public class QuestionResponse
	{
		public int Id { get; set; }
		public string Content { get; set; } = string.Empty;
		public IEnumerable<AnswerResponse> Answers { get; set; } = [];
	}
}
