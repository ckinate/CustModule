using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Domain.Events.Onboarding;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Commands;

public record UpdateCustomerCommand : IRequest<BaseResponse<bool>>
{
	public UpdateCustomerDto Item { get; set; }
}

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, BaseResponse<bool>>
{
	private readonly IApplicationDbContext _context;
	private readonly IIdentityService _identityService;

	public UpdateCustomerCommandHandler(IApplicationDbContext context, IIdentityService identityService)
	{
		_context = context;
		_identityService = identityService;
	}

	public async Task<BaseResponse<bool>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
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

		if (!string.IsNullOrEmpty(request.Item.CustomerCode))
			entity.Code = request.Item.CustomerCode;

		if (request.Item.Stage.HasValue)
			entity.Stage = GetCustomerDomainStage(request.Item.Stage.Value);

		if (request.Item.Status.HasValue)
			entity.Status = GetCustomerDomainStatus(request.Item.Status.Value);

		if(!request.Item.OnlyStatus)
			entity.AddDomainEvent(new CustomerOnboardAcceptedEvent(entity, "none",user.AdminName, user.Email));
		else
			entity.AddDomainEvent(new CustomerOnboardStatusEvent(entity, "none", user.AdminName, user.Email));

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

	private Domain.Enums.CustomerStage GetCustomerDomainStage(CustomerStage stage)
	{
		if (stage == CustomerStage.Stage1)
			return Domain.Enums.CustomerStage.Stage1;
		else if (stage == CustomerStage.Stage2)
			return Domain.Enums.CustomerStage.Stage2;
		else if (stage == CustomerStage.Stage3)
			return Domain.Enums.CustomerStage.Stage3;
		else if (stage == CustomerStage.Stage4)
			return Domain.Enums.CustomerStage.Stage4;
		else if (stage == CustomerStage.Stage5)
			return Domain.Enums.CustomerStage.Stage5;
		else
			throw new NotImplementedException();
	}
}
