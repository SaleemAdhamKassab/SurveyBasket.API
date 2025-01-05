using SurveyBasket.API.Abstractions.Result.ApiResult;

namespace SurveyBasket.API.Errors
{
	public class RoleErrors
	{
		public static readonly Error RoleNotFound = new("Role.RoleNotFound",
															  "Role is not found",
															   StatusCodes.Status404NotFound);

		public static readonly Error DuplicatedRole = new("Role.DuplicatedRole",
															  "The Role Already Exists",
															   StatusCodes.Status409Conflict);

		public static readonly Error InvalidPermissions = new("Role.InvalidPermissions",
															  "Invalid Permissions",
															   StatusCodes.Status400BadRequest);
	}
}
