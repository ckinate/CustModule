using Fintrak.CustomerPortal.Blazor.Shared.Models.CentralPay;
using Fintrak.CustomerPortal.Blazor.Shared.Models;

namespace Fintrak.CustomerPortal.Application.Common.Interfaces
{
    public interface ICentralPayIntegrationService
    {
        Task<BaseResponse<CentralPayQueryModel>> QueryTransaction(string transactionId);
    }
}
