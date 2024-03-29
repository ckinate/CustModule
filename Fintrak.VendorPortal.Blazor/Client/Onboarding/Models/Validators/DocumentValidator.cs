using FluentValidation;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class DocumentsValidator : AbstractValidator<DocumentsModel>
	{
		public DocumentsValidator(OnboardingStepService onboardingStepService)
		{
			RuleForEach(x => x.Documents).SetValidator(new DocumentValidator(onboardingStepService));
		}
	}

	public class DocumentValidator : AbstractValidator<DocumentModel>
	{
		public DocumentValidator(OnboardingStepService onboardingStepService)
		{
			RuleFor(p => p.Title)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c=> string.IsNullOrEmpty(c.LocationUrl)).When(c => onboardingStepService.CurrentStep == 7);

			RuleFor(p => p.FileData)
			   .NotNull().WithMessage("{PropertyName} is required.").When(c => string.IsNullOrEmpty(c.LocationUrl)).When(c => onboardingStepService.CurrentStep == 7);
		}
	}
}
