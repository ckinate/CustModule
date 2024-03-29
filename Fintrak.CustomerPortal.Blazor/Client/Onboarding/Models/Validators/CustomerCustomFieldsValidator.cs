using FluentValidation;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class CustomerCustomFieldsValidator : AbstractValidator<CustomFieldsModel>
	{
		public CustomerCustomFieldsValidator(OnboardingStepService onboardingStepService)
		{
			RuleForEach(x => x.CustomFields).SetValidator(new CustomerCustomFieldValidator(onboardingStepService));
		}
	}

	public class CustomerCustomFieldValidator : AbstractValidator<CustomFieldModel>
	{
		public CustomerCustomFieldValidator(OnboardingStepService onboardingStepService)
		{
			RuleFor(p => p.Field)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 8)
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.").When(c => onboardingStepService.CurrentStep == 8);

			RuleFor(p => p.Response)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => c.Compulsory).When(c => onboardingStepService.CurrentStep == 8)
			   .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.").When(c => onboardingStepService.CurrentStep == 8);
		}
	}
}
