using FluentValidation;

namespace SurveyBasket.API.Contracts.Auth
{
	public class LoginRequestValidator : AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator()
		{
			RuleFor(e => e.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(e => e.Password)
				.NotEmpty();
		}
	}
}
