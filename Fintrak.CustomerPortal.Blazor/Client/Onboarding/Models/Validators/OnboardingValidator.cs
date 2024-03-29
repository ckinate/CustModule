using FluentValidation;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
{

	public class OnboardingValidator : AbstractValidator<OnboardingModel>
	{
		public OnboardingValidator(OnboardingStepService onboardingStepService)
		{	
			RuleFor(x => x.OfficialInformation).SetValidator(new OfficialInformationValidator(onboardingStepService));
			RuleFor(x => x.ContactPersons).SetValidator(new ContactPersonsValidator(onboardingStepService));
			RuleFor(x => x.ContactChannels).SetValidator(new ContactChannelsValidator(onboardingStepService));
			RuleFor(x => x.FeeAccounts).SetValidator(new FeeAccountsValidator(onboardingStepService));
			RuleFor(x => x.CommissionAccounts).SetValidator(new CommissionAccountsValidator(onboardingStepService));
			RuleFor(x => x.Signatories).SetValidator(new SignatoriesValidator(onboardingStepService));
			RuleFor(x => x.Documents).SetValidator(new DocumentsValidator(onboardingStepService));
		}
	}
}
