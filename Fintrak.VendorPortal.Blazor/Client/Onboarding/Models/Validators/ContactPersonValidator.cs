using Fintrak.VendorPortal.Blazor.Shared.Models;
using FluentValidation;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class ContactPersonsValidator : AbstractValidator<ContactPersonsModel>
	{
		public ContactPersonsValidator(OnboardingStepService onboardingStepService)
		{
			RuleForEach(x => x.ContactPersons).SetValidator(new ContactPersonValidator(onboardingStepService));
		}
	}

	public class ContactPersonValidator : AbstractValidator<ContactPersonModel>
	{
		public ContactPersonValidator(OnboardingStepService onboardingStepService)
		{
			RuleFor(p => p.Name)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 2);

			RuleFor(p => p.Email)
			  .Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().When(c => onboardingStepService.CurrentStep == 2)
			   .Matches(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").WithMessage("{PropertyName} not valid.").When(c => onboardingStepService.CurrentStep == 2);

			RuleFor(p => p.MobilePhoneCallCode)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 2);

			RuleFor(p => p.MobilePhoneNumber)
			   //.NotEmpty().WithMessage("{PropertyName} is required.")
				//.MaximumLength(11).When(c => c.MobilePhoneCallCode == "+234" && !string.IsNullOrEmpty(c.MobilePhoneNumber)).WithMessage("{PropertyName} must not exceed 11 characters.")
			  .Must((model, phoneNumber) => PhoneValidator.ValidatePhoneNumber(model.MobilePhoneIsoCode, phoneNumber)).WithMessage("{PropertyName} is not valid.").When(c => onboardingStepService.CurrentStep == 2);

			RuleFor(p => p.Designation)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 2);
		}
	}
}
