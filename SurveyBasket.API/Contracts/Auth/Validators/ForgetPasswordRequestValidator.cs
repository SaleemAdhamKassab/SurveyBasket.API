using FluentValidation;
using SurveyBasket.API.Contracts.Users.Requests;

namespace SurveyBasket.API.Contracts.Auth.Validators
{
	public class ForgetPasswordRequestValidator : AbstractValidator<ForgetPasswordRequest>
	{
		public ForgetPasswordRequestValidator()
		{
			RuleFor(e => e.Email)
				.NotEmpty()
				.EmailAddress();
		}
	}
}
