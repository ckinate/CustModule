using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Domain.Events.Onboarding;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Commands;

public record UpdateCustomerStateCommand : IRequest<BaseResponse<bool>>
{
	public ChangeCustomerStateDto Item { get; set; }
	public string NotificationEmail { get; set; }
}

public class UpdateCustomerStateCommandHandler : IRequestHandler<UpdateCustomerStateCommand, BaseResponse<bool>>
{
	private readonly IApplicationDbContext _context;
    private readonly IIdentityService _identityService;

    public UpdateCustomerStateCommandHandler(IApplicationDbContext context,IIdentityService identityService)
	{
		_context = context;
		_identityService = identityService;

    }

	public async Task<BaseResponse<bool>> Handle(UpdateCustomerStateCommand request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<bool>();

		var entity = await _context.Customers.FirstOrDefaultAsync(c => c.Id == request.Item.CustomerId);
		if(entity == null) 
		{
			response.Success = false;
			response.Message = $"Customer with Id '{request.Item.CustomerId}' not found.";

			return response;
		}

        var user = await _identityService.GetUserAsync(entity.LoginId);
        if (user == null)
        {
            response.Success = false;
            response.Message = $"Customer with Id '{request.Item.CustomerId}' user profile not found.";

            return response;
        }

        entity.Status = GetCustomerDomainStatus(request.Item.Status);

		entity.AddDomainEvent(new CustomerOnboardCompletedEvent(entity, request.NotificationEmail, user.AdminName, user.Email ));
		await _context.SaveChangesAsync(cancellationToken);

		response.Result = true;
		return response;
	}

	private Domain.Enums.OnboardingStatus GetCustomerDomainStatus(OnboardingStatus status)
	{
		if (status == OnboardingStatus.NotStarted)
			return Domain.Enums.OnboardingStatus.NotStarted;
		else if (status == OnboardingStatus.Submitted)
			return Domain.Enums.OnboardingStatus.Submitted;
		else if (status == OnboardingStatus.Processing)
			return Domain.Enums.OnboardingStatus.Processing;
		else if (status == OnboardingStatus.Queried)
			return Domain.Enums.OnboardingStatus.Queried;
		else if (status == OnboardingStatus.Completed)
			return Domain.Enums.OnboardingStatus.Completed;
		else
			throw new NotImplementedException();
	}
}
