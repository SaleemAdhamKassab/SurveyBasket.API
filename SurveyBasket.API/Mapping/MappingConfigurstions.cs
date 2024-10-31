using Mapster;
using SurveyBasket.API.Contracts.Questions;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Mapping
{
	public class MappingConfigurstions : IRegister
	{
		public void Register(TypeAdapterConfig config)
		{
			config.NewConfig<QuestionRequest, Question>()
				.Map(dest => dest.Answers, src => src.Answers.Select(answer => new Answer { Content = answer }));
		}
	}
}
