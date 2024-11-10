namespace SurveyBasket.API.Contracts.Dashboard
{
	public class VotesPerAnswerResponse
	{
		public string Answer { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
