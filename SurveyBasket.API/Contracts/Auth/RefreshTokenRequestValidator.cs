using FluentValidation;

namespace SurveyBasket.API.Contracts.Auth
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
