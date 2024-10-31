using SurveyBasket.API.Contracts.Answers;

namespace SurveyBasket.API.Contracts.Questions
{
	public record QuestionResponse(
		int Id,
		string Content,
		IEnumerable<AnswerResponse> Answers
	);
}
