using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Domain.Events.Onboarding;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Commands;

public record SubmitLegalDocumentsCommand : IRequest<BaseResponse<bool>>
{
	public List<CustomerOnboardingDocumentDto> Item { get; set; }
}

public class SubmitLegalDocumentsCommandHandler : IRequestHandler<SubmitLegalDocumentsCommand, BaseResponse<bool>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly ICustomerIntegrationService _customerIntegrationService;

	public SubmitLegalDocumentsCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ICustomerIntegrationService customerIntegrationService)
	{
		_context = context;
		_currentUserService = currentUserService;
		_customerIntegrationService = customerIntegrationService;
	}

	public async Task<BaseResponse<bool>> Handle(SubmitLegalDocumentsCommand request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<bool>();

		var loginId = _currentUserService.UserId;
		var customer = await _context.Customers.FirstOrDefaultAsync(c => c.LoginId == loginId);

		//get invoices
		if (!string.IsNullOrEmpty(customer.Code))
		{
			var documentsResponse = await _customerIntegrationService.UploadAndSubmitSignedDocuments(customer.Code, request.Item);
			if (documentsResponse != null && documentsResponse.Success)
			{
				response.Result = documentsResponse.Result;
				response.Success = true;
			}
		}

		return response;
	}
}
