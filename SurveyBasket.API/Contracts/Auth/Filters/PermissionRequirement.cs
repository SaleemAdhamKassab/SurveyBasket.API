using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.API.Contracts.Auth.Filters
{
	public class PermissionRequirement(string permission) : IAuthorizationRequirement
	{
		public string Permission { get; } = permission;
	}
}
