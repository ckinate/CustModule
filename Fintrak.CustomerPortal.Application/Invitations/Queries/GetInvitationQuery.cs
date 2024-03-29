using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Invitations;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.InvitationPortal.Application.Invitations.Queries;

//[Authorize]
public record class GetInvitationQuery (string Code) : IRequest<BaseResponse<InvitationDto>>;

public class GetInvitationQueryHandler : IRequestHandler<GetInvitationQuery, BaseResponse<InvitationDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetInvitationQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<BaseResponse<InvitationDto>> Handle(GetInvitationQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<InvitationDto>();

		var loginId = _currentUserService.UserId;
		var entity = await _context.Invitations.FirstOrDefaultAsync(c => c.Code == request.Code);

		if (entity == null)
			throw new NotFoundException(nameof(Invitation),$"with code \"{request.Code}\"" );

		response.Result = _mapper.Map<InvitationDto>(entity);
		return response;

	}
}