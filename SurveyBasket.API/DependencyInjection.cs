using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using SurveyBasket.API.Data;
using SurveyBasket.API.Models;
using SurveyBasket.API.Services;
using SurveyBasket.API.Services.Auth;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SurveyBasket.API.Contracts.Auth;

namespace SurveyBasket.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddDependecies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllers();

			services
				.AddSwaggerConfig()
				.AddMapsterConfig()
				.AddFluentValidationConfig()
				.AddAuthConfig(configuration);

			services.AddScoped<IPollService, PollService>();

			return services;
		}

		private static IServiceCollection AddSwaggerConfig(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			return services;
		}

		private static IServiceCollection AddMapsterConfig(this IServiceCollection services)
		{
			var mappingConfig = TypeAdapterConfig.GlobalSettings;
			mappingConfig.Scan(Assembly.GetExecutingAssembly());
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
			services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

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

			return services;
		}
	}
}
