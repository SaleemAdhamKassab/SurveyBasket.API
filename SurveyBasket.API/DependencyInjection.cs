using FluentValidation;
using FluentValidation.AspNetCore;
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
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Errors;

namespace SurveyBasket.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddDependecies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddControllers();

			services.AddCors(options => options.AddDefaultPolicy(builder => builder
					.AllowAnyMethod()
					.AllowAnyHeader()
					.WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!))
			);

			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			services
				.AddSwaggerConfig()
				.AddFluentValidationConfig()
				.AddAuthConfig(configuration);

			services.AddScoped<IPollService, PollService>();
			services.AddScoped<IQuestionService, QuestionService>();
			services.AddScoped<IVoteService, VoteService>();

			services.AddExceptionHandler<GlobalExceptionHandler>();
			services.AddProblemDetails();

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
