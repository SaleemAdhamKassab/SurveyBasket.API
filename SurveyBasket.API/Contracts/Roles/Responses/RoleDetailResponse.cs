namespace SurveyBasket.API.Contracts.Roles.Responses
{
	public class RoleDetailResponse
	{
		public string Id { get; set; } = string.Empty;
		public string Name { get; set; } = string.Empty;
		public bool IsDeleted { get; set; }
		public IEnumerable<string> Permissions { get; set; } = [];
	}
}
