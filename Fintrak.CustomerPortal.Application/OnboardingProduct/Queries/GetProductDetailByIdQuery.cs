using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Utilities;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Queries
{
	public record class GetProductDetailByIdQuery(int CustomerProductId, string Hash) : IRequest<BaseResponse<OnboardProductDto>>;

	public class GetProductDetailByIdQueryHandler : IRequestHandler<GetProductDetailByIdQuery, BaseResponse<OnboardProductDto>>
	{
		private readonly IApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly IConfiguration _configuration;

		public GetProductDetailByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IConfiguration configuration)
		{
			_context = context;
			_mapper = mapper;
			_configuration = configuration;
		}

		public async Task<BaseResponse<OnboardProductDto>> Handle(GetProductDetailByIdQuery request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<OnboardProductDto>();

			//if (!ValidateQuery(request))
			//{
			//	throw new Exception("[GetProductDetailByIdQuery] - Request parameters not properly formated.");
			//}

			var customerProduct = await _context.CustomerProducts
				.Include(c => c.Customer)
				.Include(c => c.CustomerContactPerson)
				.Include(c => c.CustomerAccount)
				.Include(c => c.CustomerProductCustomFields)
				.Include(c => c.CustomerProductDocuments)
				.FirstOrDefaultAsync(c => c.Id == request.CustomerProductId);

			if (customerProduct == null)
				throw new NotFoundException(nameof(CustomerProduct), $"with id \"{request.CustomerProductId}\"");

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

			response.Result = result;

			return response;
		}

		private bool ValidateQuery(GetProductDetailByIdQuery query)
		{
			var hashMode = _configuration["HashSettings:Mode"].ToString();
			var hashKey = _configuration["HashSettings:Key"].ToString();

			var hashInput = $"{query.CustomerProductId}{hashKey}";

			if (hashMode == "Sha256")
			{
				return HashUtility.ValidateSha256Hash(query.Hash, hashInput);
			}
			else if (hashMode == "MD5")
			{
				return HashUtility.ValidateMD5Hash(query.Hash, hashInput);
			}
			else
			{
				throw new NotImplementedException("Hash mode not implemented.");
			}
		}

		private OnboardingProductStatus GetProductStatus(Domain.Enums.OnboardingProductStatus status)
		{
			if (status == Domain.Enums.OnboardingProductStatus.NotStarted)
				return OnboardingProductStatus.NotStarted;
			else if (status == Domain.Enums.OnboardingProductStatus.Submitted)
				return OnboardingProductStatus.Submitted;
			else if (status == Domain.Enums.OnboardingProductStatus.Processing)
				return OnboardingProductStatus.Processing;
			else if (status == Domain.Enums.OnboardingProductStatus.Queried)
				return OnboardingProductStatus.Queried;
			else if (status == Domain.Enums.OnboardingProductStatus.Completed)
				return OnboardingProductStatus.Completed;
			else
				throw new NotImplementedException();
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
