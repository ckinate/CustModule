using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using FluentValidation;

namespace Fintrak.CustomerPortal.Application.Onboarding.Validators
{
	public class BankAccountValidator : AbstractValidator<UpsertBankAccountDto>
	{
		public BankAccountValidator()
		{
			RuleFor(p => p.BankName)
			   .NotEmpty().WithMessage("{PropertyName} is required.")
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

			RuleFor(p => p.BankCode)
			   .NotEmpty().WithMessage("{PropertyName} is required.")
			   .MaximumLength(50).WithMessage("{PropertyName} must not exceed 50 characters.");

			RuleFor(p => p.AccountName)
			   .NotEmpty().WithMessage("{PropertyName} is required.")
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

			RuleFor(p => p.AccountNumber)
			   .NotEmpty().WithMessage("{PropertyName} is required.")
			   .MaximumLength(10).WithMessage("{PropertyName} must not exceed 10 characters.");

			//RuleFor(p => p.Country)
			//   .NotEmpty().WithMessage("{PropertyName} is required.")
			//   .MaximumLength(100).WithMessage("{PropertyName} must not exceed 100 characters.");

			//RuleFor(p => p.Validated)
			//   .Must(c=> c).WithMessage("Account must be validated.");
		}
	}
}
