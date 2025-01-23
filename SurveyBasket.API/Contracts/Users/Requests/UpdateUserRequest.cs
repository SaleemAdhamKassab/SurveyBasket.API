namespace SurveyBasket.API.Contracts.Users.Requests
{
	public class UpdateUserRequest
	{
		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public IList<string> Roles { get; set; } = [];
	}
}
