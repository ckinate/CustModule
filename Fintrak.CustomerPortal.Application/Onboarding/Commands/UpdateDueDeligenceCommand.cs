using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Domain.Events.Onboarding;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Commands;

public record UpdateDueDeligenceCommand : IRequest<BaseResponse<bool>>
{
	public UpdateDueDeligenceDto Item { get; set; }
}

public class UpdateDueDeligenceCommandHandler : IRequestHandler<UpdateDueDeligenceCommand, BaseResponse<bool>>
{
	private readonly IApplicationDbContext _context;
	private readonly IIdentityService _identityService;

	public UpdateDueDeligenceCommandHandler(IApplicationDbContext context, IIdentityService identityService)
	{
		_context = context;
		_identityService = identityService;
	}

	public async Task<BaseResponse<bool>> Handle(UpdateDueDeligenceCommand request, CancellationToken cancellationToken)
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
			response.Message = "Unable to load customer's profile.";
			return response;
		}

		entity.DueDiligenceCompleted = request.Item.DueDeligenceCompleted;

		if (entity.DueDiligenceCompleted)
		{
			entity.AddDomainEvent(new CustomerDueDiligenceCompletedEvent(entity, "none", user.AdminName, user.Email));
		}

		await _context.SaveChangesAsync(cancellationToken);

		response.Result = true;
		return response;
	}
}
