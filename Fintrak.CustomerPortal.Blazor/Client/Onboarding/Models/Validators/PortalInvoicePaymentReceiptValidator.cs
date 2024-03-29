using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;
using FluentValidation;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class PortalInvoicePaymentReceiptValidator : AbstractValidator<PortalInvoicePaymentReceiptDto>
	{
		public PortalInvoicePaymentReceiptValidator()
		{
			RuleFor(p => p.InvoiceCode).NotEmpty().WithMessage("{PropertyName} is required.");
			RuleFor(p => p.ReceiptNo).NotEmpty().WithMessage("{PropertyName} is required.");
		}
	}
}
