using Fintrak.CustomerPortal.Blazor.Shared.Models;
using FluentValidation;
using PhoneNumbers;
using System.Text.RegularExpressions;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class OfficialInformationValidator : AbstractValidator<OfficialInformationModel>
	{
		public OfficialInformationValidator(OnboardingStepService onboardingStepService)
		{

			List<string> tinPatterns = new List<string>();

			RuleFor(p => p.CompanyName)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c=> onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.RegistrationCertificateNumber)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.IncorporationDate)
			   .Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().When(c => onboardingStepService.CurrentStep == 1)
					.Must(date => date != default(DateTime) && date <= DateTime.Now)
					.WithMessage("{PropertyName} is required").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.RegisterAddress1)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.TaxIdentificationNumber)
			   .Must(c => ValidateTin(c, onboardingStepService.TinValidationPatterns)).WithMessage("{PropertyName} is invalid.").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.Country)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.State)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.City)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 1);

			//RuleFor(p => p.OfficePhoneCallCode)
			//	.NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.OfficePhoneNumber)
				//.NotEmpty().WithMessage("{PropertyName} is required.")
				.Must((model, phoneNumber) => PhoneValidator.ValidatePhoneNumber(model.OfficePhoneIsoCode, phoneNumber)).WithMessage("{PropertyName} is not valid.").When(c => onboardingStepService.CurrentStep == 1);
			//.MaximumLength(11).When(c => c.OfficePhoneCallCode == "+234" && !string.IsNullOrEmpty(c.OfficePhoneNumber)).WithMessage("{PropertyName} must not exceed 11 characters.");
			//.MaximumLength(50).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.");

			//RuleFor(p => p.MobilePhoneCallCode)
			//	.NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.MobilePhoneNumber)
				//.NotEmpty().WithMessage("{PropertyName} is required.")
				//.MaximumLength(11).When(c => c.OfficePhoneCallCode == "+234" && !string.IsNullOrEmpty(c.OfficePhoneNumber)).WithMessage("{PropertyName} must not exceed 11 characters.")
			   .Must((model, phoneNumber) => PhoneValidator.ValidatePhoneNumber(model.MobilePhoneIsoCode, phoneNumber)).WithMessage("{PropertyName} is not valid.").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.Email)
				.Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().When(c => onboardingStepService.CurrentStep == 1)
				.Matches(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
		 @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
		 @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").WithMessage("{PropertyName} not valid.").When(c => onboardingStepService.CurrentStep == 1);
			//.MaximumLength(300).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.").When(c => !string.IsNullOrEmpty(c.Email));

			RuleFor(p => p.Fax)
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.SectorId)
				.Must(c => c > 0).WithMessage("Sector is required.").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.IndustryId)
				.Must(c => c > 0).WithMessage("Industry is required.").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.StaffSize)
				.Must(c => c > 0).WithMessage("Staff size is required.").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.InstitutionTypeId)
			.Must(c => c > 0).WithMessage("Institution Type is required.").When(c => onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.ChildInstitutionTypeId)
			.Must(c => c > 0).WithMessage("Sub Institution Type is required.").When(c => c.HasChildInstitutionType && onboardingStepService.CurrentStep == 1);

			RuleFor(p => p.SettlementBankCode)
			   .NotEmpty().WithMessage("Settlement Bank is required.").When(c => onboardingStepService.CurrentStep == 1 && onboardingStepService.HasSettlementBank);

			RuleFor(p => p.SettlementBankName)
			   .NotEmpty().WithMessage("Settlement Bank is required.").When(c => onboardingStepService.CurrentStep == 1 && onboardingStepService.HasSettlementBank);

			RuleFor(p => p.ParentCode)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 1 && c.IsSubsidiary);
		}

		private bool ValidateTin(string tin, List<string> tinPatterns)
		{
			if(!string.IsNullOrEmpty(tin))
			{
                foreach (var pattern in tinPatterns)
                {
                    var regex = new Regex(pattern);
                    var result = regex.Match(tin);

                    if (result.Success)
                        return true;
                }

				return false;
            }
			else
			{
                return false;
            }
		}

		//protected async Task<bool> Validate(IJSRuntime JSRuntime, string countryCode, string phoneNumber)
		//{
		//	var result = await JSRuntime.InvokeAsync<bool>("registrationInterop.validatePhone", countryCode, phoneNumber);
		//	return result;
		//}
	}
}
