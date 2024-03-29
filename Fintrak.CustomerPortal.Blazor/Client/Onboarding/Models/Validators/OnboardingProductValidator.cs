using FluentValidation;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
{

	public class OnboardingProductValidator : AbstractValidator<OnboardingProductModel>
	{
		public OnboardingProductValidator()
		{
			RuleFor(p => p.ProductName)
			   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.ProductCode)
			   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleForEach(x => x.AdditionalInformations).SetValidator(new CustomFieldValidator());
			RuleForEach(x => x.Documents).SetValidator(new DocumentValidator(new OnboardingStepService { CurrentStep = 7 }));
		}
	}

	
}
