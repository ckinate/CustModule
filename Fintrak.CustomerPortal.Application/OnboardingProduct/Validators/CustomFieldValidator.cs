using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using FluentValidation;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Validators
{
	public class CustomFieldValidator : AbstractValidator<UpsertCustomFieldDto>
	{
		public CustomFieldValidator()
		{
			RuleFor(p => p.CustomFieldId)
				.NotNull().WithMessage("Field is required.")
				.NotEqual(0).WithMessage("{PropertyName} is required.");

			RuleFor(p => p.CustomField)
			   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Response)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c=> c.IsCompulsory);

		}
	}
}
