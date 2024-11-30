using SurveyBasket.API.Abstractions.Result.ApiResult;

namespace SurveyBasket.API.Errors
{
	public class QuestionErrors
	{
		public static readonly Error QuestionNotFound = new("Question.NotFound",
															"No question found with the given ID",
															StatusCodes.Status404NotFound);

		public static readonly Error DuplicatedQuestionContent = new("Question.DuplicatedQuestionContent",
																	  "Another question with the same content is already exists",
																	  StatusCodes.Status409Conflict);
	}
}
