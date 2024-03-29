using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using FluentValidation;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class ContactChannelsValidator : AbstractValidator<ContactChannelsModel>
	{
		public ContactChannelsValidator(OnboardingStepService onboardingStepService)
		{
			RuleForEach(x => x.ContactChannels).SetValidator(new ContactChannelValidator(onboardingStepService));
		}
	}

	public class ContactChannelValidator : AbstractValidator<ContactChannelModel>
	{
		public ContactChannelValidator(OnboardingStepService onboardingStepService)
		{
			RuleFor(p => p.Email)
			   .Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().When(c=> c.Type == ChannelType.Email && onboardingStepService.CurrentStep == 3)
			   //.MaximumLength(300).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.").When(c => c.Type == ChannelType.Email)
               .Matches(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
				@"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
				@".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").WithMessage("{PropertyName} not valid.").When(c => c.Type == ChannelType.Email && onboardingStepService.CurrentStep == 3);

			RuleFor(p => p.MobilePhoneCallCode)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => c.Type == ChannelType.Phone && onboardingStepService.CurrentStep == 3)
			   .MaximumLength(10).WithMessage("{PropertyName} must not exceed 10 characters.").When(c => c.Type == ChannelType.Phone && onboardingStepService.CurrentStep == 3);

			RuleFor(p => p.MobilePhoneNumber)
			   //.NotEmpty().WithMessage("{PropertyName} is required.").When(c => c.Type == ChannelType.Phone).When(c => c.Type == ChannelType.Phone)
			   //.MaximumLength(11).When(c => c.MobilePhoneCallCode == "+234" && !string.IsNullOrEmpty(c.MobilePhoneNumber)).WithMessage("{PropertyName} must not exceed 11 characters.")
			   .Must((model, phoneNumber) => PhoneValidator.ValidatePhoneNumber(model.MobilePhoneIsoCode, phoneNumber)).WithMessage("{PropertyName} is not valid.").When(c => c.Type == ChannelType.Phone && onboardingStepService.CurrentStep == 3);
		}
	}
}
