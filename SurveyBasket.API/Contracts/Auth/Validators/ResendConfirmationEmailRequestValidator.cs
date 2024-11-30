using FluentValidation;
using SurveyBasket.API.Contracts.Auth.Requests;

namespace SurveyBasket.API.Contracts.Auth.Validators
{
	public class ResendConfirmationEmailRequestValidator : AbstractValidator<ResendConfirmationEmailRequest>
	{
		public ResendConfirmationEmailRequestValidator()
		{
			RuleFor(e => e.Email)
				.NotEmpty()
				.EmailAddress();
		}
	}
}
