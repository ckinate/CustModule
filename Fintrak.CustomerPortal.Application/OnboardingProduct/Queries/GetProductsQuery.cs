using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using MediatR;
using Microsoft.Extensions.Configuration;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Fintrak.CustomerPortal.Application.Common.Models;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Queries
{
	public record class GetProductsQuery(string? SearchText, DateTime? StartDate, DateTime? EndDate, OnboardingProductStatus? Status, int? PageIndex = 1, int? PageSize = 10) : IRequest<BaseResponse<PaginatedList<OnboardProductDto>>>;

	public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, BaseResponse<PaginatedList<OnboardProductDto>>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		public GetProductsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper, IConfiguration configuration)
		{
			_context = context;
			_currentUserService = currentUserService;
			_mapper = mapper;
			_configuration = configuration;
		}

		public async Task<BaseResponse<PaginatedList<OnboardProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<PaginatedList<OnboardProductDto>>();

			var query = _context.CustomerProducts
				.Include(c => c.Customer)
				.Include(c => c.CustomerContactPerson)
				.Include(c => c.CustomerAccount)
				.Include(c => c.CustomerProductCustomFields)
				.Include(c => c.CustomerProductDocuments).OrderByDescending(c => c.Created).AsQueryable();

			if (!string.IsNullOrEmpty(request.SearchText))
			{
				query = query.Where(c => c.Customer.Name.Contains(request.SearchText) || c.Customer.Code.Contains(request.SearchText) || c.ProductName.Contains(request.SearchText) || c.ProductCode.Contains(request.SearchText));
			}

			if (request.Status.HasValue)
			{
				var statusSearch = request.Status.Value.GetProductDomainStatus();
				query = query.Where(c => c.Status == statusSearch);
			}

			if (request.StartDate.HasValue && request.EndDate.HasValue)
			{
				query = query.Where(c => c.Created >= request.StartDate.Value && c.Created <= request.EndDate.Value);
			}

			var customerProducts = await query.Skip((request.PageIndex.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value).AsNoTracking().ToListAsync();

			var customerProductCount = await query.CountAsync();

			var list = new List<OnboardProductDto>();

			foreach (var customerProduct in customerProducts)
			{
				var listItem = new OnboardProductDto();

				listItem.Id = customerProduct.Id;
				listItem.CustomerCode = customerProduct.Customer.Code;
				listItem.CustomerName = customerProduct.Customer.Name;
				listItem.ProductId = customerProduct.ProductId;
				listItem.ProductName = customerProduct.ProductName;
				listItem.ProductCode = customerProduct.ProductCode;
				listItem.ContactPersonId = customerProduct.CustomerContactPersonId;
				listItem.ContactPersonName = customerProduct.CustomerContactPerson.Name;
				listItem.OperationMode = GetClientOperationMode(customerProduct.OperationMode);
				listItem.AccountId = customerProduct.CustomerAccountId;
				listItem.AccountNumber = customerProduct.CustomerAccount.AccountNumber;
				listItem.Reason = customerProduct.Reason;
				listItem.Website = customerProduct.Website;
				listItem.Status = customerProduct.Status.GetProductStatus();

				//Fields
				var customFields = await _context.CustomerProductCustomFields.Where(c => c.CustomerProductId == customerProduct.Id).ToListAsync();
				foreach (var item in customFields)
				{
					listItem.CustomerProductCustomFields.Add(new UpsertCustomFieldDto
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
					listItem.CustomerProductDocuments.Add(new UpsertDocumentDto
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

				list.Add(listItem);
			}

			response.Result = new PaginatedList<OnboardProductDto>(list, customerProductCount, request.PageIndex.Value, request.PageSize.Value);

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
