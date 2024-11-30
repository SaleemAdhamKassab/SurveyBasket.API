using SurveyBasket.API.Contracts.Answers.Responses;

namespace SurveyBasket.API.Contracts.Questions.Responses
{
	public class QuestionResponse
	{
		public int Id { get; set; }
		public string Content { get; set; } = string.Empty;
		public IEnumerable<AnswerResponse> Answers { get; set; } = [];
	}
}
