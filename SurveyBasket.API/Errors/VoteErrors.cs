using SurveyBasket.API.Abstractions.Result.ApiResult;

namespace SurveyBasket.API.Errors
{
	public class VoteErrors
	{
		public static readonly Error DuplicatedVote = new("Vote.DuplicatedVote",
														  "This user has already voted for this poll.",
														  StatusCodes.Status400BadRequest);


		public static readonly Error InvalidQuestions = new("Vote.InvalidQuestions",
															"InvalidQuestions",
															StatusCodes.Status409Conflict);
	}
}