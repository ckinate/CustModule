using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Microsoft.EntityFrameworkCore;
using Fintrak.CustomerPortal.Domain.Events.OnboardingProduct;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Commands
{
	public record OnboardProductCommand : IRequest<BaseResponse<int>>
	{
		public OnboardProductDto Item { get; set; }
		public string NotificationEmail { get; set; }
	}

	public class CreateProductCommandHandler : IRequestHandler<OnboardProductCommand, BaseResponse<int>>
	{
		private readonly IApplicationDbContext _context;
		private readonly ICurrentUserService _currentUserService;
		private readonly IIdentityService _identityService;
		private readonly IFileStore _fileStore;

		public CreateProductCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService, IFileStore fileStore)
		{
			_context = context;
			_currentUserService = currentUserService;
			_identityService = identityService;
			_fileStore = fileStore;
		}

		public async Task<BaseResponse<int>> Handle(OnboardProductCommand request, CancellationToken cancellationToken)
		{
			var response = new BaseResponse<int>();

			if (request.Item.CustomerProductCustomFields.Count > 0)
			{
				if (request.Item.CustomerProductCustomFields.Any(c => c.IsCompulsory && string.IsNullOrEmpty(c.Response)))
				{
					response.Success = false;
					response.Message = $"One or more of the addition ifromation fields require response.";

					return response;
				}
			}

			if (request.Item.CustomerProductDocuments.Count > 0)
			{
				var documentTypes = request.Item.CustomerProductDocuments.Select(c => c.DocumentTypeId).Distinct().ToList();
				if (documentTypes.Count != request.Item.CustomerProductDocuments.Count)
				{
					response.Success = false;
					response.Message = $"All document type must be unique.";

					return response;
				}
			}

			var user = await _identityService.GetUserAsync(_currentUserService.UserId);
			if (user == null)
			{
				response.Success = false;
				response.Message = "Unable to load customer's profile.";
				return response;
			}

			var customer = await _context.Customers.FirstOrDefaultAsync(c => c.LoginId == _currentUserService.UserId);
			if (customer == null)
			{
				response.Success = false;
				response.Message = $"Unable to load customer for login user.";

				return response;
			}

			
			
			var entity = await _context.CustomerProducts.FirstOrDefaultAsync(c => c.Id == request.Item.Id);
			if (entity == null)
			{
				var entityWithProductExist = await _context.CustomerProducts.FirstOrDefaultAsync(c => c.CustomerId == customer.Id && c.ProductId == request.Item.ProductId);

				if (entityWithProductExist != null)
				{
					response.Success = false;
					response.Message = $"Product with already exist.";

					return response;
				}

				entity = new CustomerProduct
				{
					CustomerId = customer.Id,	
					ProductId = request.Item.ProductId,	
					ProductName = request.Item.ProductName,
					ProductCode = request.Item.ProductCode,
					CustomerContactPersonId = request.Item.ContactPersonId,
					OperationMode = GetDomainOperationMode(request.Item.OperationMode),
					CustomerAccountId = request.Item.AccountId,	
					Reason = request.Item.Reason,
					Website = request.Item.Website,	
					Status = Domain.Enums.OnboardingProductStatus.Submitted
				};

				//logo

				entity.AddDomainEvent(new ProductOnboardCompletedEvent(entity,customer.Name, request.NotificationEmail, user.AdminName, user.Email, true));
				_context.CustomerProducts.Add(entity);
			}
			else
			{
				if(entity.Status != Domain.Enums.OnboardingProductStatus.Queried)
				{
					response.Success = false;
					response.Message = $"Product not eligible for this operation.";

					return response;
				}

				var entityWithProductExist = await _context.CustomerProducts.FirstOrDefaultAsync(c => c.CustomerId == customer.Id && c.ProductId == request.Item.ProductId && c.Id != entity.Id);

				if (entityWithProductExist != null)
				{
					response.Success = false;
					response.Message = $"Product with already exist.";

					return response;
				}

				entity.Remark = "";
				entity.Status = Domain.Enums.OnboardingProductStatus.Submitted;

				entity.AddDomainEvent(new ProductOnboardCompletedEvent(entity, customer.Name, request.NotificationEmail, user.AdminName, user.Email));

				var queries = await _context.Queries.Where(c => c.ResourceType == Domain.Enums.ResourceType.CustomerProduct && c.ResourceReference == entity.Id.ToString() && c.RequireDataModification).ToListAsync();
				foreach (var query in queries)
				{
					query.RequireDataModification = false;
				}
			}

			await _context.SaveChangesAsync(cancellationToken);

			//process sub contact persons
			var existingCustomFields = await _context.CustomerProductCustomFields.Where(c => c.CustomerProductId == entity.Id).ToListAsync();
			if (existingCustomFields.Any())
			{
				_context.CustomerProductCustomFields.RemoveRange(existingCustomFields);
				await _context.SaveChangesAsync(cancellationToken);
			}

			foreach (var customField in request.Item.CustomerProductCustomFields)
			{
				var newCustomField = new CustomerProductCustomField
				{
					CustomerProductId = entity.Id,
					CustomFieldId = customField.CustomFieldId,
					CustomField = customField.CustomField,
					IsCompulsory = customField.IsCompulsory,
					Response = customField.Response,
				};

				_context.CustomerProductCustomFields.Add(newCustomField);
			}

			await _context.SaveChangesAsync(cancellationToken);

			//process documents
			var existingDocuments = await _context.CustomerProductDocuments.Where(c => c.CustomerProductId == entity.Id).ToListAsync();
			var existingDocumentIds = request.Item.CustomerProductDocuments.Where(c => c.DocumentId.HasValue).Select(c => c.DocumentId).Distinct().ToList();
			var documentsToDelete = existingDocuments.Where(c => !existingDocumentIds.Contains(c.Id)).ToList();

			if (documentsToDelete.Any())
			{
				_context.CustomerProductDocuments.RemoveRange(documentsToDelete);
				await _context.SaveChangesAsync(cancellationToken);
			}

			foreach (var document in request.Item.CustomerProductDocuments)
			{
				//process file data

				CustomerProductDocument existingDocument = null;
				if (document.DocumentId.HasValue)
					existingDocument = existingDocuments.FirstOrDefault(c => c.Id == document.DocumentId.Value);

				var locationUrl = string.Empty;
				if (document.FileData != null)
				{
					locationUrl = await _fileStore.UploadFile(document.FileData, $"{document.Title.ToLower()}_customer_{entity.Id}", document.ContentType, "customers");

					if (string.IsNullOrEmpty(locationUrl))
					{
						throw new FileUploadException(document.Title, $"Product: {entity.ProductName}");
					}
				}

				if (existingDocument != null)
				{
					existingDocument.Title = document.Title;

					if (!string.IsNullOrEmpty(locationUrl))
					{
						existingDocument.LocationUrl = locationUrl;
					}
				}
				else
				{
					var newDocument = new CustomerProductDocument
					{
						CustomerProductId = entity.Id,
						DocumentTypeId = document.DocumentTypeId,
						DocumentTypeName = document.DocumentTypeName,
						Title = document.Title,
						LocationUrl = locationUrl,
						IssueDate = document.IssueDate,
						HasExpiryDate = document.HasExpiryDate,
						ExpiryDate = document.ExpiryDate,
					};

					_context.CustomerProductDocuments.Add(newDocument);
				}
			}

			await _context.SaveChangesAsync(cancellationToken);


			response.Result = entity.Id;

			return response;
		}

		private Domain.Enums.OperationMode GetDomainOperationMode(OperationMode operationMode)
		{
			if (operationMode == OperationMode.API)
				return Domain.Enums.OperationMode.API;
			else if (operationMode == OperationMode.Portal)
				return Domain.Enums.OperationMode.Portal;
			else
				return Domain.Enums.OperationMode.Both;
		}
	}
}
