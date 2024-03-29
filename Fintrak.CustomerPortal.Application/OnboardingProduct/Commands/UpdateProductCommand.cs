using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Events.OnboardingProduct;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Commands
{
    public record UpdateProductCommand : IRequest<BaseResponse<bool>>
	{
		public UpdateProductDto Item { get; set; }
	}

	public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, BaseResponse<bool>>
	{
		private readonly IApplicationDbContext _context;
		private readonly IIdentityService _identityService;

		public UpdateProductCommandHandler(IApplicationDbContext context, IIdentityService identityService)
		{
			_context = context;
			_identityService = identityService;
		}

		public async Task<BaseResponse<bool>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<bool>();

			var entity = await _context.CustomerProducts.FirstOrDefaultAsync(c => c.Id == request.Item.CustomerProductId);
			if (entity == null)
			{
				response.Success = false;
				response.Message = $"Customer Product with Id '{request.Item.CustomerProductId}' not found.";

				return response;
			}

			var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == entity.CustomerId);
			if (customer == null)
			{
				response.Success = false;
				response.Message = $"Customer with Id '{entity.CustomerId}' not found.";

				return response;
			}

			var user = await _identityService.GetUserAsync(customer.LoginId);
			if (user == null)
			{
				response.Success = false;
				response.Message = $"Customer with Id '{customer.Id}' user profile not found.";

				return response;
			}

			if (request.Item.Status.HasValue)
			{
                entity.Status = GetProductDomainStatus(request.Item.Status.Value);
            }

			if (!string.IsNullOrEmpty(request.Item.Remark))
			{
                entity.Remark = request.Item.Remark;
            }

			if (!request.Item.OnlyStatus)
				entity.AddDomainEvent(new ProductOnboardAcceptedEvent(entity, customer.Name, "none", user.AdminName, user.Email));
			else
				entity.AddDomainEvent(new ProductOnboardStatusEvent(entity, customer.Name, "none", user.AdminName, user.Email));

			await _context.SaveChangesAsync(cancellationToken);

			response.Result = true;
			return response;
		}

		private Domain.Enums.OnboardingProductStatus GetProductDomainStatus(OnboardingProductStatus status)
		{
			if (status == OnboardingProductStatus.NotStarted)
				return Domain.Enums.OnboardingProductStatus.NotStarted;
			else if (status == OnboardingProductStatus.Submitted)
				return Domain.Enums.OnboardingProductStatus.Submitted;
			else if (status == OnboardingProductStatus.Processing)
				return Domain.Enums.OnboardingProductStatus.Processing;
			else if (status == OnboardingProductStatus.Queried)
				return Domain.Enums.OnboardingProductStatus.Queried;
			else if (status == OnboardingProductStatus.Completed)
				return Domain.Enums.OnboardingProductStatus.Completed;
			else
				throw new NotImplementedException();
		}
	}
}
