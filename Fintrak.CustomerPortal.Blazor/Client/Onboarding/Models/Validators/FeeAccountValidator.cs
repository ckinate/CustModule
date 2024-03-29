using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using FluentValidation;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class FeeAccountsValidator : AbstractValidator<BankAccountsModel>
	{
		public FeeAccountsValidator(OnboardingStepService onboardingStepService)
		{
			RuleForEach(x => x.BankAccounts).SetValidator(new FeeAccountValidator(onboardingStepService));
		}
	}

	public class FeeAccountValidator : AbstractValidator<BankAccountModel>
	{
		public FeeAccountValidator(OnboardingStepService onboardingStepService)
		{
			RuleFor(p => p.BankName)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 4 && c.AccountType == AccountType.Fee);

			RuleFor(p => p.BankCode)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 4 && c.AccountType == AccountType.Fee);

			//RuleFor(p => p.Country)
			//   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 5);

			RuleFor(p => p.AccountName)
			   .NotEmpty().WithMessage("{PropertyName} is required.")
			   .When(c => onboardingStepService.CurrentStep == 4 && c.AccountType == AccountType.Fee);

			RuleFor(p => p.AccountNumber)
			   .NotEmpty().MaximumLength(10).When(c => onboardingStepService.CurrentStep == 4 && c.AccountType == AccountType.Fee)
			   .Matches(@"^\d+$").WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 4 && c.AccountType == AccountType.Fee);
		}
	}
}
