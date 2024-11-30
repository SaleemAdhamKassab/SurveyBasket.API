using FluentValidation;
using SurveyBasket.API.Contracts.Auth.Requests;

namespace SurveyBasket.API.Contracts.Auth.Validators
{
	public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
	{
		public RefreshTokenRequestValidator()
		{
			RuleFor(e => e.Token)
				.NotEmpty()
				.EmailAddress();

			RuleFor(e => e.RefreshToken)
				.NotEmpty();
		}
	}
}
