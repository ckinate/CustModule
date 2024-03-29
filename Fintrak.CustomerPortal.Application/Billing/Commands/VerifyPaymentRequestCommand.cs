using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using MediatR;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;

namespace Fintrak.CustomerPortal.Application.Billing.Commands
{
    public record VerifyPaymentRequestCommand : IRequest<BaseResponse<CentralPayLogDto>>
    {
        public string RequestId { get; set; }
        public string NotificationEmail { get; set; }
    }

    public class VerifyPaymentRequestCommandHandler : IRequestHandler<VerifyPaymentRequestCommand, BaseResponse<CentralPayLogDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ICustomerIntegrationService _customerIntegrationService;

        public VerifyPaymentRequestCommandHandler(ICustomerIntegrationService customerIntegrationService)
        {
            _customerIntegrationService = customerIntegrationService;
        }

        public async Task<BaseResponse<CentralPayLogDto>> Handle(VerifyPaymentRequestCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<CentralPayLogDto>();

            response = await _customerIntegrationService.VerifyPaymentRequest(request.RequestId);
            return response;
        }
    }
}
