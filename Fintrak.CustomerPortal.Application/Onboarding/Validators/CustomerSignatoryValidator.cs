using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using FluentValidation;

namespace Fintrak.CustomerPortal.Application.Onboarding.Validators
{
	public class CustomerSignatoryValidator : AbstractValidator<UpsertSignatoryDto>
	{
		public CustomerSignatoryValidator()
		{
			RuleFor(p => p.FirstName)
			   .NotEmpty().WithMessage("{PropertyName} is required.");

			//RuleFor(p => p.MiddleName)
			//   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.LastName)
			   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Name)
			   .NotEmpty().WithMessage("{PropertyName} is required.")
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

            RuleFor(p => p.Email)
              .Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
               .Matches(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").WithMessage("{PropertyName} not valid.");

            RuleFor(p => p.MobilePhoneCallCode)
               .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.MobilePhoneNumber)
               .NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Designation)
               .NotEmpty().WithMessage("{PropertyName} is required.");

        }
    }
}
