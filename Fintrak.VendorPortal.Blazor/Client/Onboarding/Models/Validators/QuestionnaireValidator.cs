using FluentValidation;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class QuestionnairesValidator : AbstractValidator<QuestionnairesModel>
	{
		public QuestionnairesValidator(OnboardingStepService onboardingStepService)
		{
			RuleForEach(x => x.Questionnaires).SetValidator(new QuestionnaireValidator(onboardingStepService));
		}
	}

	public class QuestionnaireValidator : AbstractValidator<QuestionnaireModel>
	{
		public QuestionnaireValidator(OnboardingStepService onboardingStepService)
		{
			RuleFor(p => p.Question)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 6)
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.").When(c => onboardingStepService.CurrentStep == 6);

			RuleFor(p => p.Response)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c=> c.Compulsory).When(c => onboardingStepService.CurrentStep == 6)
			   .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.").When(c => onboardingStepService.CurrentStep == 6);
		}
	}
}
