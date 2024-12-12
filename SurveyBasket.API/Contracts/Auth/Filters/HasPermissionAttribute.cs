using Microsoft.AspNetCore.Authorization;

namespace SurveyBasket.API.Contracts.Auth.Filters
{
	public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission)
	{
	}
}
