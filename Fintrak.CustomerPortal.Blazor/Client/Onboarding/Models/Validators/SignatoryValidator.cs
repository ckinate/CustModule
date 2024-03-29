using FluentValidation;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class SignatoriesValidator : AbstractValidator<SignatoriesModel>
	{
		public SignatoriesValidator(OnboardingStepService onboardingStepService)
		{
			RuleForEach(x => x.Signatories).SetValidator(new SignatoryValidator(onboardingStepService));
		}
	}

	public class SignatoryValidator : AbstractValidator<SignatoryModel>
	{
		public SignatoryValidator(OnboardingStepService onboardingStepService)
		{
			RuleFor(p => p.FirstName)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 6)
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.").When(c => onboardingStepService.CurrentStep == 6);

			//RuleFor(p => p.MiddleName)
			//   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 6)
			//   .MaximumLength(200).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.").When(c => onboardingStepService.CurrentStep == 6);

			RuleFor(p => p.LastName)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 6)
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.").When(c => onboardingStepService.CurrentStep == 6);

			//RuleFor(p => p.Name)
			//   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 6)
			//   .MaximumLength(200).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.").When(c => onboardingStepService.CurrentStep == 6);

			RuleFor(p => p.Email)
			  .Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().When(c => onboardingStepService.CurrentStep == 6)
			   .Matches(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
		 @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
		 @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").WithMessage("{PropertyName} not valid.").When(c => onboardingStepService.CurrentStep == 6);

			RuleFor(p => p.MobileNumberCallCode)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 6);

			RuleFor(p => p.MobileNumber)
			  .Must((model, phoneNumber) => PhoneValidator.ValidatePhoneNumber(model.MobileNumberIsoCode, phoneNumber)).WithMessage("{PropertyName} is not valid.").When(c => onboardingStepService.CurrentStep == 6);

			RuleFor(p => p.Designation)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => onboardingStepService.CurrentStep == 6);


		}
	}
}
