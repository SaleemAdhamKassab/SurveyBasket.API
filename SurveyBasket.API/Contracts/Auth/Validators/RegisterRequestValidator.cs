using FluentValidation;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Contracts.Auth.Requests;

namespace SurveyBasket.API.Contracts.Auth.Validators
{
	public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
	{
		public RegisterRequestValidator()
		{
			RuleFor(e => e.FirstName).NotEmpty().MaximumLength(100);

			RuleFor(e => e.LastName).NotEmpty().MaximumLength(100);

			RuleFor(e => e.Email).NotEmpty().EmailAddress();

			RuleFor(e => e.Password)
				.NotEmpty()
				.Matches(RegexPatterns.Password)
				.WithMessage("Password Rules: (8+ Digits , contains lowercase-uppercase-Non Alphanumric");
		}
	}
}
