using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

[Authorize]
public record class GetCustomerAccountsQuery(AccountType? AccountType) : IRequest<BaseResponse<List<CustomerAccountDto>>>;

public class GetCustomerAccountsQueryHandler : IRequestHandler<GetCustomerAccountsQuery, BaseResponse<List<CustomerAccountDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetCustomerAccountsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<BaseResponse<List<CustomerAccountDto>>> Handle(GetCustomerAccountsQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<List<CustomerAccountDto>>();

		var loginId = _currentUserService.UserId;

		var query = _context.CustomerAccounts
			.Where(c => c.Customer.LoginId == loginId );

		List<CustomerAccount> accounts;

		if (request.AccountType.HasValue)
		{
			query = query.Where(c=> c.AccountType == request.AccountType.Value.GetDomainAccountType());
		}

		accounts = await query.ToListAsync();

		if (accounts == null)
			throw new NotFoundException(nameof(CustomerAccount),$"with user id \"{loginId}\"" );

		response.Result = _mapper.Map<List<CustomerAccountDto>>(accounts);

		return response;
	}
}