using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using MediatR;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using System.Reflection.Metadata;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Queries
{
	[Authorize]
	public record class GetProductDetailQuery(int CustomerProductId) : IRequest<BaseResponse<OnboardProductDto>>;

	public class GetProductDetailQueryHandler : IRequestHandler<GetProductDetailQuery, BaseResponse<OnboardProductDto>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;

		public GetProductDetailQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
		{
			_context = context;
			_currentUserService = currentUserService;
			_mapper = mapper;
		}

		public async Task<BaseResponse<OnboardProductDto>> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<OnboardProductDto>();

			var loginId = _currentUserService.UserId;
			var customerProduct = await _context.CustomerProducts
				.Include(c => c.Customer)
				.Include(c => c.CustomerContactPerson)
				.Include(c => c.CustomerAccount)
				.Include(c => c.CustomerProductCustomFields)
				.Include(c => c.CustomerProductDocuments)
				.FirstOrDefaultAsync(c => c.Id == request.CustomerProductId);

			//if (customer == null)
			//	throw new NotFoundException(nameof(Product),$"with user id \"{loginId}\"" );
			if (customerProduct != null)
			{
				var result = new OnboardProductDto();

				result.Id = customerProduct.Id;
				result.CustomerCode = customerProduct.Customer.Code;
				result.CustomerName = customerProduct.Customer.Name;
				result.ProductId = customerProduct.ProductId;
				result.ProductName = customerProduct.ProductName;
				result.ProductCode = customerProduct.ProductCode;
				result.ContactPersonId = customerProduct.CustomerContactPersonId;
				result.ContactPersonName = customerProduct.CustomerContactPerson.Name;
				result.OperationMode = GetClientOperationMode(customerProduct.OperationMode);
				result.AccountId = customerProduct.CustomerAccountId;
				result.AccountNumber = customerProduct.CustomerAccount.AccountNumber;
				result.Reason = customerProduct.Reason;
				result.Website = customerProduct.Website;
				result.Status = customerProduct.Status.GetProductStatus();

				//Fields
				var customFields = await _context.CustomerProductCustomFields.Where(c => c.CustomerProductId == customerProduct.Id).ToListAsync();
				foreach (var item in customFields)
				{
					result.CustomerProductCustomFields.Add(new UpsertCustomFieldDto
					{
						CustomFieldId = item.CustomFieldId,
						CustomField = item.CustomField,
						IsCompulsory = item.IsCompulsory,
						Response = item.Response,
					});
				}

				//Documents
				var documents = await _context.CustomerProductDocuments.Where(c => c.CustomerProductId == customerProduct.Id).ToListAsync();
			    foreach (var item in documents)
				{
					result.CustomerProductDocuments.Add(new UpsertDocumentDto
					{
						DocumentTypeId = item.DocumentTypeId.Value,
						DocumentTypeName = item.DocumentTypeName,
						Title = item.Title,
						LocationUrl = item.LocationUrl,
						IssueDate = item.IssueDate,
						HasExpiryDate = item.HasExpiryDate,
						ExpiryDate = item.ExpiryDate,
						DocumentId = item.Id
					});
				}

				var query = await _context.Queries.Where(c => c.ResourceType == Domain.Enums.ResourceType.CustomerProduct && c.ResourceReference ==customerProduct.Id.ToString() && c.RequireDataModification).FirstOrDefaultAsync();
				if (query != null)
				{
					result.CanUpdate = true;
				}

				response.Result = result;
			}

			return response;
		}

		private OperationMode GetClientOperationMode(Domain.Enums.OperationMode operationMode)
		{
			if (operationMode == Domain.Enums.OperationMode.API)
				return OperationMode.API;
			else if (operationMode == Domain.Enums.OperationMode.Portal)
				return OperationMode.Portal;
			else
				return OperationMode.Both;
		}
	}
}
