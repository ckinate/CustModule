using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Queries
{
	[Authorize]
	public record class GetProductQuery : IRequest<BaseResponse<CustomerProductDto>>;

	public class GetProductQueryHandler : IRequestHandler<GetProductQuery, BaseResponse<CustomerProductDto>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;

		public GetProductQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
		{
			_context = context;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<BaseResponse<CustomerProductDto>> Handle(GetProductQuery request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<CustomerProductDto>();

			var loginId = _currentUserService.UserId;

			var customerProduct = await _context.CustomerProducts
				.Include(c => c.Customer)
				.Include(c => c.CustomerContactPerson)
				.Include(c => c.CustomerAccount)
				.Include(c => c.CustomerProductCustomFields)
				.Include(c => c.CustomerProductDocuments)
				.FirstOrDefaultAsync(c => c.Customer.LoginId == loginId);

			if (customerProduct == null)
				throw new NotFoundException(nameof(CustomerProduct), $"with user id \"{loginId}\"");

			response.Result = _mapper.Map<CustomerProductDto>(customerProduct);

			return response;
		}
	}
}
