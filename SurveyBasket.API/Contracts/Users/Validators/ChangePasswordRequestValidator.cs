using FluentValidation;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.API.Contracts.Users.Requests;

namespace SurveyBasket.API.Contracts.Questions.Validators
{
	public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
	{
		public ChangePasswordRequestValidator()
		{
			RuleFor(e => e.CurrentPassword)
				.NotEmpty();

			RuleFor(e => e.NewPassword)
				.NotEmpty()
				.Matches(RegexPatterns.Password)
				.WithMessage("Password Rules: (8+ Digits , contains lowercase-uppercase-Non Alphanumric")
				.NotEqual(e => e.CurrentPassword)
				.WithMessage("New Password cannot be same as the current password");
		}
	}
}
