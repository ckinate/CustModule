using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using FluentValidation;

namespace Fintrak.CustomerPortal.Application.Onboarding.Validators
{
	public class DocumentValidator : AbstractValidator<UpsertDocumentDto>
	{
		public DocumentValidator()
		{
			RuleFor(p => p.DocumentTypeId)
				.NotNull().WithMessage("Document type is required.").When(c => string.IsNullOrEmpty(c.LocationUrl))
				.NotEqual(0).WithMessage("{PropertyName} is required.").When(c => string.IsNullOrEmpty(c.LocationUrl));

			RuleFor(p => p.DocumentTypeName)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => string.IsNullOrEmpty(c.LocationUrl));

			RuleFor(p => p.Title)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => string.IsNullOrEmpty(c.LocationUrl));

			RuleFor(p => p.FileData)
			   .NotNull().WithMessage("{PropertyName} is required.").When(c => string.IsNullOrEmpty(c.LocationUrl));

			RuleFor(p => p.IssueDate)
			   .NotNull().WithMessage("{PropertyName} is required.").When(c => string.IsNullOrEmpty(c.LocationUrl) && c.HasIssueDate);

			RuleFor(p => p.ExpiryDate)
			   .NotNull().WithMessage("{PropertyName} is required.").When(c => string.IsNullOrEmpty(c.LocationUrl) && c.HasExpiryDate);
		}
	}
}
