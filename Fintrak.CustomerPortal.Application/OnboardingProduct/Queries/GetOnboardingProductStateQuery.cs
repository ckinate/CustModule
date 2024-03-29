using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Queries
{
	[Authorize]
	public record class GetOnboardingProductStateQuery : IRequest<BaseResponse<OnboardingProductStatus>>;

	public class GetOnboardingProductStateQueryHandler : IRequestHandler<GetOnboardingProductStateQuery, BaseResponse<OnboardingProductStatus>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;

		public GetOnboardingProductStateQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
		{
			_context = context;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<BaseResponse<OnboardingProductStatus>> Handle(GetOnboardingProductStateQuery request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<OnboardingProductStatus>();

			var loginId = _currentUserService.UserId;
			var customerProduct = await _context.CustomerProducts.FirstOrDefaultAsync(c => c.Customer.LoginId == loginId);

			if (customerProduct == null)
				response.Result = OnboardingProductStatus.NotStarted;
			else
				response.Result = customerProduct.Status.GetProductStatus();

			return response;
		}
	}
}
