namespace SurveyBasket.API.Contracts.Votes.Requests
{
	public class VoteRequest
	{
		public List<VoteAnswerRequest> VoteAnswers { get; set; } = [];
	}
}
