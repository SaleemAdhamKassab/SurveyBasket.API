using Microsoft.AspNetCore.Authorization;
using SurveyBasket.API.Abstractions.Consts;

namespace SurveyBasket.API.Contracts.Auth.Filters
{
	public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
	{
		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
		{
			var user = context.User.Identity;

			if (!user.IsAuthenticated || !context.User.Claims.Any(e => e.Value == requirement.Permission && e.Type == Permissions.Type))
				return;

			context.Succeed(requirement);
			return;
		}
	}
}
