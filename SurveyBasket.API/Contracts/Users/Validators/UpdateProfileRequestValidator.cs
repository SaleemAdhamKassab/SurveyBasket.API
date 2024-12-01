using FluentValidation;
using SurveyBasket.API.Contracts.Users.Requests;

namespace SurveyBasket.API.Contracts.Questions.Validators
{
	public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
	{
		public UpdateProfileRequestValidator()
		{
			RuleFor(e => e.FirstName).NotEmpty().Length(3, 100);
			RuleFor(e => e.LastName).NotEmpty().Length(3, 100);
		}
	}
}
