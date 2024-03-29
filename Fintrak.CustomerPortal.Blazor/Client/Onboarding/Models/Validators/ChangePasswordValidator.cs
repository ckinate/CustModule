using FluentValidation;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models.Validators
{
	public class ChangePasswordValidator : AbstractValidator<ChangePasswordModel>
	{
		public ChangePasswordValidator()
		{
			RuleFor(p => p.OldPassword)
			   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.NewPassword)
			   .NotEmpty().WithMessage("{PropertyName} is required.");

			RuleFor(p => p.ConfirmPassword)
			   .NotEmpty().WithMessage("{PropertyName} is required.")
			   .Equal(c=> c.NewPassword);
		}
	}
}
