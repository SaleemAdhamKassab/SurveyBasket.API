using FluentValidation;

namespace SurveyBasket.API.Contracts.Auth
{
	public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
	{
		public RefreshTokenRequestValidator()
		{
			RuleFor(e => e.token)
				.NotEmpty()
				.EmailAddress();

			RuleFor(e => e.refreshToken)
				.NotEmpty();
		}
	}
}
