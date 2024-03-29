using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Queries;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Queries.Queries
{
	[Authorize]
	public record class GetQueriesQuery(bool LoadAllQueries = false) : IRequest<BaseResponse<List<QueryDto>>>;

	public class GetQueriesQueryHandler : IRequestHandler<GetQueriesQuery, BaseResponse<List<QueryDto>>>
	{
		private readonly IApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;

		public GetQueriesQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		{
			_context = context;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}

		public async Task<BaseResponse<List<QueryDto>>> Handle(GetQueriesQuery request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<List<QueryDto>>();

			var loginId = _currentUserService.UserId;
			var customer = await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.LoginId == loginId);

			List<Query> entities = default!;

			if (request.LoadAllQueries)
			{
				entities = await _context.Queries.Include(c=> c.Customer).OrderByDescending(c => c.EntryDate).Where(c=> c.CustomerId == customer.Id).ToListAsync();
			}
			else
			{
				entities = await _context.Queries.Include(c => c.Customer).OrderByDescending(c => c.EntryDate).Where(c => c.IsPending && c.CustomerId == customer.Id).ToListAsync();
			}

			response.Result = _mapper.Map<List<QueryDto>>(entities);
			return response;

		}
	}
}
