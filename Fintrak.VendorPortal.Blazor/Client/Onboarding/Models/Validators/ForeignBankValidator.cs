using FluentValidation;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class ForeignBanksValidator : AbstractValidator<ForeignBanksModel>
	{
		public ForeignBanksValidator(OnboardingStepService onboardingStepService)
		{
			RuleForEach(x => x.ForeignBanks).SetValidator(new ForeignBankValidator(onboardingStepService));
		}
	}

	public class ForeignBankValidator : AbstractValidator<ForeignBankModel>
	{
		public ForeignBankValidator(OnboardingStepService onboardingStepService)
		{
			RuleFor(p => p.BankName)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 5);

			RuleFor(p => p.BankCode)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 5);

			RuleFor(p => p.Country)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 5);

			RuleFor(p => p.AccountName)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 5);

			RuleFor(p => p.AccountNumber)
			   .NotEmpty().When(c => onboardingStepService.CurrentStep == 5)
			   .Matches(@"^\d+$").WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 5);
		}
	}
}
