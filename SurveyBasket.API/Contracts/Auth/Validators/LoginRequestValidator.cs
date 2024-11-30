using FluentValidation;
using SurveyBasket.API.Contracts.Auth.Requests;

namespace SurveyBasket.API.Contracts.Auth.Validators
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
