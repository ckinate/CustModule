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
public record class GetCustomerQuery : IRequest<BaseResponse<CustomerDto>>;

public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, BaseResponse<CustomerDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetCustomerQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<BaseResponse<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<CustomerDto>();

		var loginId = _currentUserService.UserId;
		var customer = await _context.Customers
			.Include(c=> c.CustomerContactPersons)
			.Include(c => c.CustomerContactChannels)
			.Include(c => c.CustomerAccounts)
			.Include(c => c.CustomerSignatories)
			.Include(c => c.CustomerDocuments)
			.Include(c=> c.Parent)
			.FirstOrDefaultAsync(c => c.LoginId == loginId);

		if (customer == null)
			throw new NotFoundException(nameof(Customer),$"with user id \"{loginId}\"" );

		response.Result = _mapper.Map<CustomerDto>(customer);

		return response;
	}
}