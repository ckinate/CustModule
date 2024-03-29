using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Queries;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Domain.Events.Queries;

namespace Fintrak.CustomerPortal.Application.Queries.Commands
{
	[Authorize]
	public class ResponseToQueryCommand : IRequest<BaseResponse<bool>>
	{
		public ResponseToQueryDto Item { get; set; }
		public string NotificationEmail { get; set; }
	}

	public class ResponseToQueryCommandValidator : AbstractValidator<ResponseToQueryCommand>
	{
		public ResponseToQueryCommandValidator()
		{
			RuleFor(v => v.Item).NotNull();
			RuleFor(v => v.Item.Response).MaximumLength(500).NotEmpty();
		}
	}

	public class ResponseToQueryCommandHandler : IRequestHandler<ResponseToQueryCommand, BaseResponse<bool>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IConfiguration _configuration;

		public ResponseToQueryCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IConfiguration configuration)
		{
			_context = context;
			_currentUserService = currentUserService;
			_configuration = configuration;
		}

		public async Task<BaseResponse<bool>> Handle(ResponseToQueryCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<bool>();

			var entity = await _context.Queries.Include(c=> c.Customer).FirstOrDefaultAsync(c => c.Id == request.Item.QueryId);
			if (entity == null)
			{
				throw new NotFoundException(nameof(Query), request.Item.QueryId);
			}

            var customer = await GetCustomer(entity);
            if (customer == null)
            {
                throw new NotFoundException(nameof(Customer), entity.ResourceReference);
            }

            entity.QueryResponse = request.Item.Response;
			entity.ResponseDate = DateTime.Now;
			entity.IsPending = false;
			entity.RequireDataModification = request.Item.RequireDataModification;

			entity.AddDomainEvent(new RespondToQueryEvent(entity, request.NotificationEmail));

			if (!request.Item.RequireDataModification)
			{
				customer.Status = entity.PreviousStatus;
			}
			
			await _context.SaveChangesAsync(cancellationToken);

			response.Result = true;

			return response;
		}

        private async Task<Customer> GetCustomer(Query query)
        {
            Customer customer = default!;

            if (query.ResourceType == Domain.Enums.ResourceType.Customer)
            {
                var customerId = int.Parse(query.ResourceReference);
                customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
            }
			else if (query.ResourceType == Domain.Enums.ResourceType.CustomerProduct)
			{
				var customerProductId = int.Parse(query.ResourceReference);

				var productCustomer = await _context.CustomerProducts.FirstOrDefaultAsync(c => c.Id == customerProductId);
				if(productCustomer != null)
				{
					customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == productCustomer.Customer.Id);
				}
				
			}

			return customer;
        }
    }
}
