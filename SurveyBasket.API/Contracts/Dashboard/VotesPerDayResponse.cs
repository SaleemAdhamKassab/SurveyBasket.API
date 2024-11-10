namespace SurveyBasket.API.Contracts.Dashboard
{
	public class VotesPerDayResponse
	{
		public DateOnly Date { get; set; }
		public int NumberOfVotes { get; set; }
	}
}
