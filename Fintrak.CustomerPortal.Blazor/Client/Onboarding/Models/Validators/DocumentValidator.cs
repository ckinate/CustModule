using FluentValidation;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
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
			RuleFor(p => p.DocumentTypeId)
				.NotNull().WithMessage("Document type is required.").When(c => string.IsNullOrEmpty(c.LocationUrl) && onboardingStepService.CurrentStep == 7)
				.NotEqual(0).WithMessage("{PropertyName} is required.").When(c => string.IsNullOrEmpty(c.LocationUrl) && onboardingStepService.CurrentStep == 7);

			RuleFor(p => p.Title)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c=> string.IsNullOrEmpty(c.LocationUrl)).When(c => onboardingStepService.CurrentStep == 7);

			RuleFor(p => p.FileData)
			   .NotNull().WithMessage("{PropertyName} is required.").When(c => string.IsNullOrEmpty(c.LocationUrl)).When(c => onboardingStepService.CurrentStep == 7);

			RuleFor(p => p.IssueDate)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => string.IsNullOrEmpty(c.LocationUrl) && c.HasIssueDate).When(c => onboardingStepService.CurrentStep == 7);

			RuleFor(p => p.ExpiryDate)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => string.IsNullOrEmpty(c.LocationUrl) && c.HasExpiryDate).When(c => onboardingStepService.CurrentStep == 7);
		}
	}
}
