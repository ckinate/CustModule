using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Invitations;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Invitations.Queries;

public record class GetInvitationsQuery (string SearchText, 
	InvitationUsedSearch Status, DateTime? StartDate, DateTime? EndDate) : IRequest<BaseResponse<List<InvitationDto>>>;

public class GetInvitationsQueryHandler : IRequestHandler<GetInvitationsQuery, BaseResponse<List<InvitationDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetInvitationsQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<BaseResponse<List<InvitationDto>>> Handle(GetInvitationsQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<List<InvitationDto>>();

		List<Invitation> entities = default!;

		var query = _context.Invitations.OrderByDescending(c => c.EntryDate).AsQueryable();

		if(request.Status == InvitationUsedSearch.Used) 
		{
            query = query.Where(c => c.Used);
        }
		else if (request.Status == InvitationUsedSearch.Unused)
        {
            query = query.Where(c => !c.Used);
        }

        if (!string.IsNullOrEmpty(request.SearchText))
		{
			query = query.Where(c=> c.CompanyName.Contains(request.SearchText));           
		}

        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            query = query.Where(c => c.EntryDate >= request.StartDate.Value && c.EntryDate <= request.EndDate.Value);
        }

        entities = await query.ToListAsync();
        response.Result = _mapper.Map<List<InvitationDto>>(entities);
		return response;

	}
}