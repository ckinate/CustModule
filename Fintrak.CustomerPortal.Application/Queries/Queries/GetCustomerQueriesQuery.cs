using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Queries;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace Fintrak.CustomerPortal.Application.Queries.Queries
{
	public record class GetCustomerQueriesQuery(int? CustomerId, bool LoadAllQueries = false) : IRequest<BaseResponse<List<QueryDto>>>;

	public class GetCustomerQueriesQueryHandler : IRequestHandler<GetCustomerQueriesQuery, BaseResponse<List<QueryDto>>>
	{
		private readonly IApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly ICurrentUserService _currentUserService;

		public GetCustomerQueriesQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
		{
			_context = context;
			_mapper = mapper;
			_currentUserService = currentUserService;
		}

		public async Task<BaseResponse<List<QueryDto>>> Handle(GetCustomerQueriesQuery request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<List<QueryDto>>();
			List<Query> entities = default!;

			var query = _context.Queries.Where(c=> c.ResourceType == Domain.Enums.ResourceType.Customer).Include(c => c.Customer).OrderByDescending(c => c.EntryDate).AsQueryable();

			if (request.CustomerId.HasValue)
			{
				query = query.Where(c => c.CustomerId == request.CustomerId.Value);
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
