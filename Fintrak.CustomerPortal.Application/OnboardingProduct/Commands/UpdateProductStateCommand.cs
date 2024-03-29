using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Domain.Events.OnboardingProduct;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Commands
{
	public record UpdateProductStateCommand : IRequest<BaseResponse<bool>>
	{
		public ChangeProductStateDto Item { get; set; }
		public string NotificationEmail { get; set; }
	}

	public class UpdateProductStateCommandHandler : IRequestHandler<UpdateProductStateCommand, BaseResponse<bool>>
	{
		private readonly IApplicationDbContext _context;
		private readonly IIdentityService _identityService;

		public UpdateProductStateCommandHandler(IApplicationDbContext context, IIdentityService identityService)
		{
			_context = context;
			_identityService = identityService;

		}

		public async Task<BaseResponse<bool>> Handle(UpdateProductStateCommand request, CancellationToken cancellationToken)
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
				response.Message = $"Customer with Id '{entity.CustomerId}' user profile not found.";

				return response;
			}

			if(request.Item.Status == OnboardingProductStatus.Queried)
                entity.Remark = request.Item.Remark;
			else
                entity.Remark = "";

            entity.Status = GetProductDomainStatus(request.Item.Status);

			entity.AddDomainEvent(new ProductOnboardCompletedEvent(entity, customer.Name, request.NotificationEmail, user.AdminName, user.Email));
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
