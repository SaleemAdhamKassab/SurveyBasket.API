using FluentValidation;

namespace SurveyBasket.API.Contracts.Auth
{
	public class QuestionRequestValidator : AbstractValidator<LoginRequest>
	{
		public QuestionRequestValidator()
		{
			RuleFor(e => e.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(e => e.Password)
				.NotEmpty();
		}
	}
}
