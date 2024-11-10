namespace SurveyBasket.API.Contracts.Votes
{
	public class VoteRequest
	{
		public List<VoteAnswerRequest> VoteAnswers { get; set; } = [];
	}
}
