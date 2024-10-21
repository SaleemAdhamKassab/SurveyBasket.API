namespace SurveyBasket.API.Abstractions
{
	public record Error(string Code, string Description)
	{
		public static readonly Error None = new Error(string.Empty, string.Empty);
	}
}
