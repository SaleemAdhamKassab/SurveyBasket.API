using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Abstractions.Result.ApiResult;
using SurveyBasket.API.Contracts.Roles.Requests;
using SurveyBasket.API.Contracts.Roles.Responses;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Models;
using SurveyBasket.API.Models.Data;

namespace SurveyBasket.API.Services.RolesService
{
	public class RoleService(RoleManager<ApplicationRole> roleManager, ApplicationDbContext db) : IRoleService
	{
		private readonly ApplicationDbContext _db = db;
		private readonly RoleManager<ApplicationRole> _roleManager = roleManager;


		public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisables = false)
		{
			var roles = await _roleManager.Roles
				.Where(e => !e.IsDefault && (!e.IsDeleted || includeDisables == true))
				.Select(e => new RoleResponse
				{
					Id = e.Id,
					Name = e.Name!,
					IsDeleted = e.IsDeleted
				})
				.ToListAsync();

			return roles;
		}
		public async Task<Result<RoleDetailResponse>> GetAsync(string Id)
		{
			var role = await _roleManager.FindByIdAsync(Id);

			if (role is null)
				return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);

			var permissions = await _roleManager.GetClaimsAsync(role);

			var result = new RoleDetailResponse
			{
				Id = role.Id,
				Name = role.Name!,
				IsDeleted = role.IsDeleted,
				Permissions = permissions.Select(e => e.Value)
			};

			return Result.Success(result);
		}
		public async Task<Result<RoleDetailResponse>> AddAsync(CreateRoleRequest request)
		{
			var isRoleExists = await _roleManager.RoleExistsAsync(request.Name);

			if (isRoleExists)
				return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);

			var allowedPermissions = Permissions.GetAllPermissions();

			if (request.Permissions.Except(allowedPermissions).Any())
				return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);

			var role = new ApplicationRole
			{
				Name = request.Name,
				IsDeleted = false,
				IsDefault = false,
				ConcurrencyStamp = Guid.NewGuid().ToString()
			};

			var result = await _roleManager.CreateAsync(role);

			if (!result.Succeeded)
			{
				var error = result.Errors.First();
				return Result.Failure<RoleDetailResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
			}

			var permissions = request.Permissions.Select(e => new IdentityRoleClaim<string>
			{
				ClaimType = Permissions.Type,
				ClaimValue = e,
				RoleId = role.Id
			});

			await _db.AddRangeAsync(permissions);
			await _db.SaveChangesAsync();

			var response = new RoleDetailResponse
			{
				Id = role.Id,
				Name = role.Name!,
				IsDeleted = role.IsDeleted,
				Permissions = request.Permissions
			};

			return Result.Success(response);
		}
		public async Task<Result> UpdateAsync(string id, CreateRoleRequest request)
		{
			var role = await _roleManager.FindByIdAsync(id);
			if (role is not null)
				return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);

			var isRoleExists = await _roleManager.Roles.AnyAsync(e => e.Name == request.Name && e.Id != id);
			if (isRoleExists)
				return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);

			var allowedPermissions = Permissions.GetAllPermissions();
			if (request.Permissions.Except(allowedPermissions).Any())
				return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);

			role!.Name = request.Name;
			var result = await _roleManager.UpdateAsync(role);

			if (!result.Succeeded)
			{
				var error = result.Errors.First();
				return Result.Failure<RoleDetailResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
			}

			var currentPermissions = await _db.RoleClaims
				.Where(e => e.RoleId == id && e.ClaimType == Permissions.Type)
				.Select(e => e.ClaimValue)
				.ToListAsync();

			var newPermissions = request.Permissions
				.Except(currentPermissions)
				.Select(e => new IdentityRoleClaim<string>
				{
					ClaimType = Permissions.Type,
					ClaimValue = e,
					RoleId = role.Id
				});

			var removedPermissions = currentPermissions.Except(request.Permissions);

			await _db.RoleClaims
				.Where(e => e.RoleId == id && removedPermissions.Contains(e.ClaimValue))
				.ExecuteDeleteAsync();

			await _db.AddRangeAsync(newPermissions);
			await _db.SaveChangesAsync();

			return Result.Success();
		}
	}
}









