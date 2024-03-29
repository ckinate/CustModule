using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Queries
{
	public record class GetMyProductsQuery() : IRequest<BaseResponse<List<OnboardProductDto>>>;

	public class GetMyProductsQueryHandler : IRequestHandler<GetMyProductsQuery, BaseResponse<List<OnboardProductDto>>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		public GetMyProductsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper, IConfiguration configuration)
		{
			_context = context;
			_currentUserService = currentUserService;
			_mapper = mapper;
			_configuration = configuration;
		}

		public async Task<BaseResponse<List<OnboardProductDto>>> Handle(GetMyProductsQuery request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<List<OnboardProductDto>>();

			var loginId = _currentUserService.UserId;

			var customerProducts = await _context.CustomerProducts
				.Include(c => c.Customer)
				.Include(c => c.CustomerContactPerson)
				.Include(c => c.CustomerAccount)
				.OrderByDescending(c => c.ProductName)
				.Where(c=> c.Customer.LoginId == loginId).ToListAsync();

			var list = new List<OnboardProductDto>();

			//foreach (var customerProduct in customerProducts)
			//{
			//	var listItem = new OnboardProductDto();

			//	//
			//	listItem.Id = customerProduct.Id;
			//	listItem.Status = customerProduct.Status.GetProductStatus();

			//	list.Add(listItem);
			//}

			response.Result = _mapper.Map<List<OnboardProductDto>>(customerProducts);

			return response;
		}
	}
}
