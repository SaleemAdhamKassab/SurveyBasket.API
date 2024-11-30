namespace SurveyBasket.API.Contracts.Votes.Requests
{
	public class VoteAnswerRequest
	{
		public int QuestionId { get; set; }
		public int AnswerId { get; set; }
	}
}