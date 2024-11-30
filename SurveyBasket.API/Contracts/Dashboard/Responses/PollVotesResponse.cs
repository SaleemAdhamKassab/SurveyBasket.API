namespace SurveyBasket.API.Contracts.Dashboard.Responses
{
	public class PollVotesResponse
	{
		public string PollTitle { get; set; } = string.Empty;
		public IEnumerable<VoteResponse> VoteResponses { get; set; } = [];
	}
}
