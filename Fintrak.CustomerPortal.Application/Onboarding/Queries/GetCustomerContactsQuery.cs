using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

[Authorize]
public record class GetCustomerContactsQuery : IRequest<BaseResponse<List<ContactPersonDto>>>;

public class GetCustomerContactsQueryHandler : IRequestHandler<GetCustomerContactsQuery, BaseResponse<List<ContactPersonDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetCustomerContactsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<BaseResponse<List<ContactPersonDto>>> Handle(GetCustomerContactsQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<List<ContactPersonDto>>();

		var loginId = _currentUserService.UserId;

		var customer = await _context.CustomerContactPersons
			.Where(c => c.Customer.LoginId == loginId).ToListAsync();

		if (customer == null)
			throw new NotFoundException(nameof(CustomerContactPerson),$"with user id \"{loginId}\"" );

		response.Result = _mapper.Map<List<ContactPersonDto>>(customer);

		return response;
	}
}