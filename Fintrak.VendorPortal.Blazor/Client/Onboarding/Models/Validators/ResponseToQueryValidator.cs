using Fintrak.VendorPortal.Blazor.Shared.Models.Queries;
using FluentValidation;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class ResponseToQueryValidator : AbstractValidator<ResponseToQueryDto>
	{
		public ResponseToQueryValidator()
		{
			RuleFor(p => p.Response)
			   .NotEmpty().WithMessage("{PropertyName} is required.")
               .MaximumLength(500).WithMessage("{PropertyName} cannot be more 500 characters.");
        }
	}
}
