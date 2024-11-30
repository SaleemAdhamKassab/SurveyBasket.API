using FluentValidation;
using SurveyBasket.API.Contracts.Auth.Requests;

namespace SurveyBasket.API.Contracts.Auth.Validators
{
	public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
	{
		public ConfirmEmailRequestValidator()
		{
			RuleFor(e => e.UserId).NotEmpty();

			RuleFor(e => e.Token).NotEmpty();
		}
	}
}
