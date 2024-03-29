using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

[Authorize]
public record class GetCustomerCustomFieldsQuery : IRequest<BaseResponse<List<CustomFieldResponseDto>>>;

public class GetCustomerCustomFieldsQueryHandler : IRequestHandler<GetCustomerCustomFieldsQuery, BaseResponse<List<CustomFieldResponseDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
    private readonly ICustomerIntegrationService _customerIntegrationService;

	public GetCustomerCustomFieldsQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, ICustomerIntegrationService customerIntegrationService)
	{
		_context = context;
		_currentUserService = currentUserService;
        _customerIntegrationService = customerIntegrationService;
	}

	public async Task<BaseResponse<List<CustomFieldResponseDto>>> Handle(GetCustomerCustomFieldsQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<List<CustomFieldResponseDto>>();

		var loginId = _currentUserService.UserId;

        var customFieldResponse = await _customerIntegrationService.GetCustomerCustomFields();

        if(customFieldResponse != null && customFieldResponse.Success)
        {
            var customerCustomFields = await _context.CustomerCustomFields.Where(c => c.Customer.LoginId == loginId)
           .OrderByDescending(p => p.IsCompulsory).ToListAsync();

            var fieldResponses = new List<CustomFieldResponseDto>();
            if(customFieldResponse != null && customFieldResponse.Result != null)
            {
				foreach (var customField in customFieldResponse.Result)
				{
					var customerCustomField = customerCustomFields.FirstOrDefault(c => c.CustomFieldId == customField.Id);

					fieldResponses.Add(new CustomFieldResponseDto
					{
						Id = customField.Id,
						Name = customField.Name,
						Tag = customField.Tag,
						Order = customField.Order,
						IsCompulsory = customField.IsCompulsory,
						Response = customerCustomField != null ? customerCustomField.Response : ""
					});
				}
			}

            response.Result = fieldResponses;
        }

        return response;
	}
}