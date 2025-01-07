using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Contracts.Roles.Requests;
using SurveyBasket.API.Contracts.Roles.Responses;

namespace SurveyBasket.API.Services.RolesService
{
	public interface IRoleService
	{
		Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisables = false);
		Task<Result<RoleDetailResponse>> GetAsync(string Id);
		Task<Result<RoleDetailResponse>> AddAsync(CreateRoleRequest request);
		Task<Result> UpdateAsync(string id, CreateRoleRequest request);
		Task<Result> ToggleStatusAsync(string id);
	}
}
