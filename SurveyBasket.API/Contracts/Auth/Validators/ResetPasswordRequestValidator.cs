using FluentValidation;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Contracts.Auth.Requests;

namespace SurveyBasket.API.Contracts.Auth.Validators
{
	public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
	{
		public ResetPasswordRequestValidator()
		{
			RuleFor(e => e.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(e => e.Token)
				.NotEmpty();

			RuleFor(e => e.NewPassword)
				.NotEmpty()
				.Matches(RegexPatterns.Password)
				.WithMessage("Password Rules: (8+ Digits , contains lowercase-uppercase-Non Alphanumric");
		}
	}
}