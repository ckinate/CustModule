using FluentValidation;
using System.Security.Cryptography.X509Certificates;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Models.Validators
{

	public class OnboardingValidator : AbstractValidator<OnboardingModel>
	{
		public OnboardingValidator(OnboardingStepService onboardingStepService)
		{	
			RuleFor(x => x.OfficialInformation).SetValidator(new OfficialInformationValidator(onboardingStepService));
			RuleFor(x => x.ContactPersons).SetValidator(new ContactPersonsValidator(onboardingStepService));
			RuleFor(x => x.ContactChannels).SetValidator(new ContactChannelsValidator(onboardingStepService));
			RuleFor(x => x.LocalBank).SetValidator(new LocalBankValidator(onboardingStepService));
			RuleFor(x => x.ForeignBanks).SetValidator(new ForeignBanksValidator(onboardingStepService));
			RuleFor(x => x.Questionnaires).SetValidator(new QuestionnairesValidator(onboardingStepService));
			RuleFor(x => x.Documents).SetValidator(new DocumentsValidator(onboardingStepService));
		}
	}
}
