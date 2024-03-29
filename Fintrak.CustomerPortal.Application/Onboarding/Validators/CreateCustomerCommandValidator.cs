using Fintrak.CustomerPortal.Application.Onboarding.Commands;
using FluentValidation;

namespace Fintrak.CustomerPortal.Application.Onboarding.Validators
{
	public class CreateCustomerCommandValidator : AbstractValidator<OnboardCustomerCommand>
	{
		public CreateCustomerCommandValidator()
		{
			RuleFor(v => v.Item).NotNull();

			RuleFor(p => p.Item.Name)
				   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.RegistrationCertificateNumber)
			   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.IncorporationDate)
			   .Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
					.Must(date => date != default(DateTime) && date <= DateTime.Now)
					.WithMessage("{PropertyName} is required");

			RuleFor(p => p.Item.RegisterAddress1)
			   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.TaxIdentificationNumber).NotEmpty().WithMessage("{PropertyName} is required.");

            RuleFor(p => p.Item.Country)
			   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.OfficePhoneCallCode)
				.NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.OfficePhoneNumber)
				.NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.MobilePhoneCallCode)
				.NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.MobilePhoneNumber)
				.NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.Email)
				.Cascade(CascadeMode.StopOnFirstFailure).NotEmpty()
				.Matches(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
		 @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
		 @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").WithMessage("{PropertyName} not valid.");

			RuleFor(p => p.Item.Fax)
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

			RuleFor(p => p.Item.SectorId)
				.NotNull().WithMessage("Category is required.")
				.NotEqual(0).WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.SectorName)
				.NotEmpty().WithMessage("{PropertyName} is required.")
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

			RuleFor(p => p.Item.IndustryId)
				.NotNull().WithMessage("Sub Category(s) is required.");

			RuleFor(p => p.Item.IndustryName)
				.NotEmpty().WithMessage("Sub Category(s) is required.");

			RuleFor(p => p.Item.ParentCode)
				.NotEmpty().When(c=> c.Item.IsSubsidiary).WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.ParentName)
				.NotEmpty().When(c => c.Item.IsSubsidiary).WithMessage("{PropertyName} is required.");

			RuleForEach(x => x.Item.ContactPersons).SetValidator(new ContactPersonValidator());

			RuleForEach(x => x.Item.ContactChannels).SetValidator(new ContactChannelValidator());

			RuleForEach(x => x.Item.BankAccounts).SetValidator(new BankAccountValidator());

			RuleForEach(x => x.Item.Signatories).SetValidator(new CustomerSignatoryValidator());

			RuleForEach(x => x.Item.Documents).SetValidator(new DocumentValidator());
		}
	}
}
