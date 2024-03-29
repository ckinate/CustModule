using FluentValidation;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class LocalBankValidator : AbstractValidator<LocalBankModel>
	{
		public LocalBankValidator(OnboardingStepService onboardingStepService)
		{
			RuleFor(p => p.BankName)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => c.IsCompulsory && onboardingStepService.CurrentStep == 4);

			RuleFor(p => p.BankCode)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => c.IsCompulsory && onboardingStepService.CurrentStep == 4);

			RuleFor(p => p.AccountName)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => c.IsCompulsory && onboardingStepService.CurrentStep == 4);

			RuleFor(p => p.AccountNumber)
				.Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().When(c => c.IsCompulsory && onboardingStepService.CurrentStep == 4)
			   .MaximumLength(10).WithMessage("{PropertyName} is not valid.").When(c => c.IsCompulsory && onboardingStepService.CurrentStep == 4)
			   .Matches(@"^\d+$").When(c => c.IsCompulsory && onboardingStepService.CurrentStep == 4);

			RuleFor(p => p.Validated)
			   .Must(c => c).WithMessage("Account must be validated.").When(c => c.IsCompulsory && onboardingStepService.CurrentStep == 4);
		}
	}
}
