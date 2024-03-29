using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Queries;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Queries.Queries
{
	public record class GetCustomerProductQueriesQuery(int? CustomerProductId, bool LoadAllQueries = false) : IRequest<BaseResponse<List<QueryDto>>>;

	public class GetCustomerProductQueriesQueryHandler : IRequestHandler<GetCustomerProductQueriesQuery, BaseResponse<List<QueryDto>>>
	{
		private readonly IApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;

		public GetCustomerProductQueriesQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		{
			_context = context;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}

		public async Task<BaseResponse<List<QueryDto>>> Handle(GetCustomerProductQueriesQuery request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<List<QueryDto>>();
			List<Query> entities = default!;

			var query = _context.Queries.Include(c => c.Customer).Where(c=> c.ResourceType == Domain.Enums.ResourceType.CustomerProduct).OrderByDescending(c => c.EntryDate).AsQueryable();

			if (request.CustomerProductId.HasValue)
			{
				query = query.Where(c => c.ResourceReference == request.CustomerProductId.Value.ToString());
			}
			
			if (!request.LoadAllQueries)
			{
				entities = await query.Where(c => c.IsPending).ToListAsync();
			}
			else
			{
                entities = await query.ToListAsync();
            }

			response.Result = _mapper.Map<List<QueryDto>>(entities);
			return response;

		}
	}
}
