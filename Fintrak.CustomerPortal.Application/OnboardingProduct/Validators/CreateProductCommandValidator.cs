using Fintrak.CustomerPortal.Application.Onboarding.Validators;
using Fintrak.CustomerPortal.Application.OnboardingProduct.Commands;
using FluentValidation;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Validators
{
	public class CreateProductCommandValidator : AbstractValidator<OnboardProductCommand>
	{
		public CreateProductCommandValidator()
		{
			RuleFor(v => v.Item).NotNull();

			RuleFor(p => p.Item.ProductId)
				.NotNull().WithMessage("Product is required.")
				.NotEqual(0).WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.ProductName)
				.NotEmpty().WithMessage("{PropertyName} is required.")
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

			RuleFor(p => p.Item.ProductCode)
				.NotEmpty().WithMessage("{PropertyName} is required.")
			   .MaximumLength(200).WithMessage("{PropertyName} must not exceed 200 characters.");

			RuleFor(p => p.Item.ContactPersonId)
				.NotNull().WithMessage("Contact Person is required.")
				.NotEqual(0).WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.OperationMode)
				.NotNull().WithMessage("Operation mode is required.");

			RuleFor(p => p.Item.AccountId)
				.NotNull().WithMessage("Account is required.")
				.NotEqual(0).WithMessage("{PropertyName} is required.");

			RuleFor(p => p.Item.Website)
			   .MaximumLength(300).WithMessage("{PropertyName} must not exceed 300 characters.");

			RuleFor(p => p.Item.Reason)
			   .MaximumLength(500).WithMessage("{PropertyName} must not exceed 500 characters.");

			RuleForEach(x => x.Item.CustomerProductCustomFields).SetValidator(new CustomFieldValidator());

			RuleForEach(x => x.Item.CustomerProductDocuments).SetValidator(new DocumentValidator());
		}
	}
}
