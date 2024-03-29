using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using FluentValidation;

namespace Fintrak.CustomerPortal.Application.Onboarding.Validators
{
	public class ContactChannelValidator : AbstractValidator<UpsertContactChannelDto>
	{
		public ContactChannelValidator()
		{
			RuleFor(p => p.Email)
			   .Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().When(c => c.Type == ChannelType.Email)
			   //.MaximumLength(300).WithMessage("{PropertyName} must not exceed {ComparisonValue} characters.").When(c => c.Type == ChannelType.Email)
			   .Matches(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
				@"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
				@".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$").WithMessage("{PropertyName} not valid.").When(c => c.Type == ChannelType.Email);

			RuleFor(p => p.MobilePhoneCallCode)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => c.Type == ChannelType.Phone)
			   .MaximumLength(10).WithMessage("{PropertyName} must not exceed 10 characters.").When(c => c.Type == ChannelType.Phone);

			RuleFor(p => p.MobilePhoneNumber)
			   .NotEmpty().WithMessage("{PropertyName} is required.").When(c => c.Type == ChannelType.Phone).When(c => c.Type == ChannelType.Phone);
		}
	}
}
