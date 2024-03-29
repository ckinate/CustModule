using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using MediatR;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;

namespace Fintrak.CustomerPortal.Application.Billing.Commands
{
    public record CreateInvoicePaymentRequestCommand : IRequest<BaseResponse<CentralPayLogDto>>
    {
        public int InvoiceId { get; set; }
        public string NotificationEmail { get; set; }
    }

    public class CreateInvoicePaymentRequestCommandHandler : IRequestHandler<CreateInvoicePaymentRequestCommand, BaseResponse<CentralPayLogDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly ICustomerIntegrationService _customerIntegrationService;

        public CreateInvoicePaymentRequestCommandHandler(ICustomerIntegrationService customerIntegrationService)
        {
            _customerIntegrationService = customerIntegrationService;
        }

        public async Task<BaseResponse<CentralPayLogDto>> Handle(CreateInvoicePaymentRequestCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseResponse<CentralPayLogDto>();

            response = await _customerIntegrationService.CreatePaymentRequest(request.InvoiceId);
            return response;
        }
    }
}
