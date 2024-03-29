using FluentValidation;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class CustomFieldValidator : AbstractValidator<UpsertCustomFieldDto>
	{
		public CustomFieldValidator()
		{
			RuleFor(p => p.CustomField)
			   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Response)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => c.IsCompulsory);
		}
	}
}
