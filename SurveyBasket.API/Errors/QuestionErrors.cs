using SurveyBasket.API.Abstractions;

namespace SurveyBasket.API.Errors
{
	public class QuestionErrors
	{
		public static readonly Error QuestionNotFound = new("Question.NotFound", "No question found with the given ID");

		public static readonly Error DuplicatedQuestionContent = new("Question.DuplicatedQuestionContent", "Another question with the same content is already exists");
	}
}
