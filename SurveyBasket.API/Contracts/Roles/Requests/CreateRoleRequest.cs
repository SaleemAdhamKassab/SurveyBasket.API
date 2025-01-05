namespace SurveyBasket.API.Contracts.Roles.Requests
{
	public class CreateRoleRequest
	{
		public string Name { get; set; } = string.Empty;
		public IEnumerable<string> Permissions { get; set; } = [];
	}
}
