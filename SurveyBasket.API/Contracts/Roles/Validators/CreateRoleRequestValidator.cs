using FluentValidation;
using SurveyBasket.API.Contracts.Roles.Requests;

namespace SurveyBasket.API.Contracts.Questions.Validators
{
	public class CreateRoleRequestValidator : AbstractValidator<CreateRoleRequest>
	{
		public CreateRoleRequestValidator()
		{
			RuleFor(e => e.Name)
				.NotEmpty()
				.Length(3, 200);

			RuleFor(e => e.Permissions)
				.NotNull()
				.NotEmpty();

			RuleFor(e => e.Permissions)
				.Must(e => e.Distinct().Count() == e.Count())
				.WithMessage("you cannot duplicated permissions for the same role")
				.When(e => e.Permissions != null);
		}
	}
}
