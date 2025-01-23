using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Abstractions.ApiResult;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Abstractions.Result.ApiResult;
using SurveyBasket.API.Contracts.Users.Requests;
using SurveyBasket.API.Contracts.Users.Responses;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Models;
using SurveyBasket.API.Models.Data;
using SurveyBasket.API.Services.RolesService;

namespace SurveyBasket.API.Services.UsersService
{
	public class UserService(
		UserManager<ApplicationUser> userManager,
		IRoleService roleService,
		ApplicationDbContext db) : IUserService
	{
		private readonly ApplicationDbContext _db = db;
		private readonly UserManager<ApplicationUser> _userManager = userManager;
		private readonly IRoleService _roleService = roleService;


		public async Task<IEnumerable<UserResponse>> GetAllAsync()
		{
			var result = await (from u in _db.Users
								join ur in _db.UserRoles
								on u.Id equals ur.UserId
								join r in _db.Roles
								on ur.RoleId equals r.Id into roles
								where !roles.Any(x => x.Name == DefaultRoles.Member)
								select new
								{
									u.Id,
									u.FirstName,
									u.LastName,
									u.Email,
									u.IsDisabled,
									Roles = roles.Select(x => x.Name!).ToList()
								}
				)
				.GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.IsDisabled })
				.Select(u => new UserResponse
				{
					Id = u.Key.Id,
					FirstName = u.Key.FirstName,
					LastName = u.Key.LastName,
					Email = u.Key.Email,
					IsDisabled = u.Key.IsDisabled,
					Roles = u.SelectMany(x => x.Roles)
				})
			   .ToListAsync(); ;

			return result;
		}
		public async Task<Result<UserResponse>> GetAsync(string userId)
		{
			var user = await _userManager.FindByIdAsync(userId);

			if (user is null)
				return Result.Failure<UserResponse>(UserErrors.UserNotFound);

			var userRoles = await _userManager.GetRolesAsync(user);

			var result = new UserResponse
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email!,
				IsDisabled = user.IsDisabled,
				Roles = userRoles
			};

			return Result.Success(result);
		}
		public async Task<Result<UserResponse>> AddAsync(CreateUserRequest request)
		{
			var emailIsExists = await _userManager.FindByEmailAsync(request.Email);

			if (emailIsExists is not null)
				return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail);

			var allowedRoles = await _roleService.GetAllAsync(false);

			if (request.Roles.Except(allowedRoles.Select(e => e.Name)).Any())
				return Result.Failure<UserResponse>(UserErrors.InvalidRoles);

			var user = new ApplicationUser
			{
				FirstName = request.FirstName,
				LastName = request.LastName,
				UserName = request.Email,
				Email = request.Email,
				EmailConfirmed = true
			};

			var createResult = await _userManager.CreateAsync(user, request.Password);

			if (!createResult.Succeeded)
			{
				var error = createResult.Errors.First();
				return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
			}

			await _userManager.AddToRolesAsync(user, request.Roles);

			var result = new UserResponse
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				IsDisabled = user.IsDisabled,
				Roles = request.Roles
			};

			return Result.Success(result);
		}
		public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
		{
			var user = await _userManager.Users
				.Where(e => e.Id == userId)
				.Select(e => new UserProfileResponse
				{
					FirstName = e.FirstName,
					LastName = e.LastName,
					Email = e.Email!,
					UserName = e.UserName!
				})
				.SingleAsync();

			return Result.Success(user!);
		}
		public async Task<Result> UpdateProfileAsync(string userId, UpdateProfileRequest request)
		{
			//var user = await _userManager.FindByIdAsync(userId);

			//user!.FirstName = request.FirstName;
			//user!.LastName = request.LastName;

			//await _userManager.UpdateAsync(user);

			await _userManager.Users
				.Where(e => e.Id == userId)
				.ExecuteUpdateAsync(prop => prop
									.SetProperty(e => e.FirstName, request.FirstName)
									.SetProperty(e => e.LastName, request.LastName));

			return Result.Success();
		}
		public async Task<Result> ChangePasswordAsync(string userId, ChangePasswordRequest request)
		{
			var user = await _userManager.FindByIdAsync(userId);

			var result = await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);

			if (!result.Succeeded)
			{
				var error = result.Errors.First();
				return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
			}

			return Result.Success();
		}
	}
}  