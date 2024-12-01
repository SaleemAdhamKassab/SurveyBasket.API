namespace SurveyBasket.API.Contracts.Users.Requests
{
	public class UpdateProfileRequest
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
	}
}
