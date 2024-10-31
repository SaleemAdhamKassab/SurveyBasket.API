using FluentValidation;

namespace SurveyBasket.API.Contracts.Questions
{
	public class QuestionRequestValidator : AbstractValidator<QuestionRequest>
	{
		public QuestionRequestValidator()
		{
			RuleFor(e => e.Content).NotEmpty().Length(3, 1000);

			RuleFor(e => e.Answers)
			.NotEmpty()
			.Must(e => e != null && e.Count > 1)
			.WithMessage("Question should has at least 2 answers")
			.Must(e => e.Distinct().Count() == e.Count)
			.WithMessage("You cannot add duplicated answers for the same question")
			.When(e => e.Answers != null);
		}
	}
}
