using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using FluentValidation;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class CommissionAccountsValidator : AbstractValidator<BankAccountsModel>
	{
		public CommissionAccountsValidator(OnboardingStepService onboardingStepService)
		{
			RuleForEach(x => x.BankAccounts).SetValidator(new CommissionAccountValidator(onboardingStepService));
		}
	}

	public class CommissionAccountValidator : AbstractValidator<BankAccountModel>
	{
		public CommissionAccountValidator(OnboardingStepService onboardingStepService)
		{
			RuleFor(p => p.BankName)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c =>  onboardingStepService.CurrentStep == 5 && c.AccountType == AccountType.Commission);

			RuleFor(p => p.BankCode)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 5 && c.AccountType == AccountType.Commission);

			//RuleFor(p => p.Country)
			//   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 5);

			RuleFor(p => p.AccountName)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c =>  onboardingStepService.CurrentStep == 5 && c.AccountType == AccountType.Commission);

			RuleFor(p => p.AccountNumber)
			   .NotEmpty().MaximumLength(10).When(c => onboardingStepService.CurrentStep == 5 && c.AccountType == AccountType.Commission)
			   .Matches(@"^\d+$").WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 5 && c.AccountType == AccountType.Commission);
		}
	}
}
