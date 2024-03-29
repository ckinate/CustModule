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
public record class GetBasicCustomerQuery : IRequest<BaseResponse<BasicCustomerDto>>;

public class GetBasicCustomerQueryHandler : IRequestHandler<GetBasicCustomerQuery, BaseResponse<BasicCustomerDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetBasicCustomerQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<BaseResponse<BasicCustomerDto>> Handle(GetBasicCustomerQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<BasicCustomerDto>();

		var loginId = _currentUserService.UserId;
		var customer = await _context.Customers.Include(c=> c.Parent)
			.FirstOrDefaultAsync(c => c.LoginId == loginId);

		if (customer == null)
			throw new NotFoundException(nameof(Customer),$"with user id \"{loginId}\"" );

		response.Result = _mapper.Map<BasicCustomerDto>(customer);

		return response;
	}
}