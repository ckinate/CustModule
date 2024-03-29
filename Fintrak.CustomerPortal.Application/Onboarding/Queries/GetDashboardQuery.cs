using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

[Authorize]
public record class GetDashboardQuery : IRequest<BaseResponse<DashboardDto>>;

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, BaseResponse<DashboardDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly ICustomerIntegrationService _customerIntegrationService;
	private readonly IMapper _mapper;

	public GetDashboardQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ICustomerIntegrationService customerIntegrationService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_customerIntegrationService = customerIntegrationService;
		_mapper = mapper;
	}

	public async Task<BaseResponse<DashboardDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<DashboardDto> { Result = new() };

		var loginId = _currentUserService.UserId;

		var customer = await _context.Customers.FirstOrDefaultAsync(c => c.LoginId == loginId);
		if (customer == null)
			response.Result.OnboardingStatus.Status = OnboardingStatus.NotStarted;
		else
			response.Result.OnboardingStatus.Status = customer.Status.GetCustomerStatus();

		var customerProducts = await _context.CustomerProducts
			.Include(c => c.Customer)
			.Include(c => c.CustomerContactPerson)
			.Include(c => c.CustomerAccount)
			.OrderByDescending(c => c.ProductName)
			.Where(c => c.Customer.LoginId == loginId).ToListAsync();

		response.Result.Products = _mapper.Map<List<OnboardProductDto>>(customerProducts);

		//get tracker
		var trackerResponse = await _customerIntegrationService.GetRecentTrackers(customer.Code);
		if(trackerResponse != null && trackerResponse.Success)
		{
			response.Result.Trackers = trackerResponse.Result;
		}
		
		var list = new List<OnboardProductDto>();

		//foreach (var customerProduct in customerProducts)
		//{
		//	var listItem = new OnboardProductDto();

		//	//
		//	listItem.Id = customerProduct.Id;
		//	listItem.Status = customerProduct.Status.GetProductStatus();

		//	list.Add(listItem);
		//}

		return response;
	}
}