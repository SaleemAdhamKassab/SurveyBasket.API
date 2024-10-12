using FluentValidation;
using FluentValidation.AspNetCore;
using Mapster;
using SurveyBasket.API.Services;
using System.Reflection;

namespace SurveyBasket.API
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddDependecies(this IServiceCollection services)
		{
			services.AddControllers();

			services
				.AddSwaggerServices()
				.AddMapsterConfig()
				.AddFluentValidation();

			services.AddScoped<IPollService, PollService>();

			return services;
		}

		public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
		{
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen();
			return services;
		}

		public static IServiceCollection AddMapsterConfig(this IServiceCollection services)
		{
			var mappingConfig = TypeAdapterConfig.GlobalSettings;
			mappingConfig.Scan(Assembly.GetExecutingAssembly());
			//services.AddSingleton<IMapper>(new Mapper(mappingConfig));
			return services;
		}

		public static IServiceCollection AddFluentValidation(this IServiceCollection services)
		{
			services.AddFluentValidationAutoValidation().AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
			return services;
		}
	}
}
