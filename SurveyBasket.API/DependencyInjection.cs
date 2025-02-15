using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using SurveyBasket.API.Models;
using SurveyBasket.API.Services.Auth;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Errors;
using SurveyBasket.API.Services.DashboardsService;
using SurveyBasket.API.Services.PollsService;
using SurveyBasket.API.Services.QuestionsService;
using SurveyBasket.API.Services.VotesService;
using SurveyBasket.API.Models.Data;
using SurveyBasket.API.Settings;
using Microsoft.AspNetCore.Identity.UI.Services;
using SurveyBasket.API.Services.EmailService;
using Hangfire;
using SurveyBasket.API.Services.PollsNotificationService;
using SurveyBasket.API.Services.UsersService;
using Microsoft.AspNetCore.Authorization;
using SurveyBasket.API.Contracts.Auth.Filters;
using SurveyBasket.API.Services.RolesService;
using System.Threading.RateLimiting;

namespace SurveyBasket.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddDependecies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllers();
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
			services.AddSwaggerConfig().AddFluentValidationConfig().AddAuthConfig(configuration);
			services.AddScoped<IPollService, PollService>();
			services.AddScoped<IQuestionService, QuestionService>();
			services.AddScoped<IVoteService, VoteService>();
			services.AddScoped<IDashboardService, DashboardService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IEmailSender, EmailSender>();
			services.AddScoped<IPollsNotificationService, PollsNotificationService>();
			services.AddScoped<IRoleService, RoleService>();
			services.AddExceptionHandler<GlobalExceptionHandler>();
			services.AddProblemDetails();
			services.Configure<MailOptions>(configuration.GetSection(nameof(MailOptions)));
			services.AddHttpContextAccessor();
			services.AddHangfireConfig(configuration);
			services.AddHealthCheckConfig(configuration);
			services.AddRateLimitConfig();
			services.AddCors(options => options.AddDefaultPolicy(builder => builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!))
			);
			return services;
		}

		private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			//services.AddSwaggerGen();
			services.AddSwaggerGen(setup =>
			{

				// Include 'SecurityScheme' to use JWT Authentication
				var jwtSecurityScheme = new OpenApiSecurityScheme
				{
					BearerFormat = "JWT",
					Name = "JWT Authentication",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = JwtBearerDefaults.AuthenticationScheme,
					Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

					Reference = new OpenApiReference
					{
						Id = JwtBearerDefaults.AuthenticationScheme,
						Type = ReferenceType.SecurityScheme
					}
				};

				setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

				setup.AddSecurityRequirement(new OpenApiSecurityRequirement
					{
						{ jwtSecurityScheme, Array.Empty<string>() }
					});
			});
			return services;
		}

		private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
		{
			services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			return services;
		}

		private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IAuthService, AuthService>();

			services.AddIdentity<ApplicationUser, ApplicationRole>()
				.AddEntityFrameworkStores<ApplicationDbContext>()
				.AddDefaultTokenProviders();

			services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
			services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

			//JWT
			services.AddSingleton<IJwtProvider, JwtProvider>();
			//services.Configure<JwtOptions>(configuration.GetSection(nameof(JwtOptions.SectionName)));
			services.AddOptions<JwtOptions>()
				.BindConfiguration(JwtOptions.SectionName)
				.ValidateDataAnnotations()
				.ValidateOnStart();
			JwtOptions? jwtSettings = configuration.GetSection("Jwt").Get<JwtOptions>();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
					ValidIssuer = jwtSettings?.Issuer,
					ValidAudience = jwtSettings?.Audience
				};
			});

			services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequiredLength = 6;
				options.SignIn.RequireConfirmedEmail = true;
				options.User.RequireUniqueEmail = true;
			});

			return services;
		}

		private static IServiceCollection AddHangfireConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHangfire(config => config
				.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
				.UseSimpleAssemblyNameTypeSerializer()
				.UseRecommendedSerializerSettings()
				.UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection"))
			);

			services.AddHangfireServer();

			return services;
		}

		private static IServiceCollection AddHealthCheckConfig(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddHealthChecks()
				.AddSqlServer("database", configuration.GetConnectionString("DefaultConnection")!)
				.AddHangfire(options =>
				{
					options.MinimumAvailableServers = 1;
					options.MaximumJobsFailed = 1;
				});

			return services;
		}

		private static IServiceCollection AddRateLimitConfig(this IServiceCollection services)
		{
			services.AddRateLimiter(options =>
			{
				options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
				options.AddPolicy("IpLimit", httpContext =>
				RateLimitPartition.GetFixedWindowLimiter(
					partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
					factory: _ => new FixedWindowRateLimiterOptions
					{
						PermitLimit = 2,
						Window = TimeSpan.FromSeconds(20)
					}
				));
			});

			return services;
		}
	}
}