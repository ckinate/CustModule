using Fintrak.CustomerPortal.Blazor.Shared.Models;
using System.Net.Http.Json;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;

namespace Fintrak.CustomerPortal.Blazor.Client.Services
{
	public class BillingService : IBillingService
	{
		private readonly HttpClient _http;

		public BillingService(HttpClient http)
		{
			_http = http;
		}

		public async Task<BaseResponse<List<BillInvoiceDto>>> GetMyInvoices(int pageIdex, int pageSize)
		{
			var result = new BaseResponse<List<BillInvoiceDto>>();

			try
			{
				result = await _http.GetFromJsonAsync<BaseResponse<List<BillInvoiceDto>>>($"api/Billings/GetMyInvoices?pageIndex={pageIdex}&pageSize={pageSize}");
			}
			catch (Exception exception)
			{
				result.Success = false;
				result.Message = exception.Message;
				//exception.Redirect();
			}

			return result;
		}

        public async Task<BaseResponse<BillInvoiceDto>> GetInvoice(int invoiceId)
        {
            var result = new BaseResponse<BillInvoiceDto>();

            try
            {
                var response = await _http.GetFromJsonAsync<BaseResponse<BillInvoiceDto>>($"api/Billings/GetInvoice?invoiceId={invoiceId}");

                if (response != null && response.Success)
                {
                    if (response.Result != null)
                    {
                        result = response;
                    }
                }
            }
            catch (Exception exception)
            {
                result.Success = false;
                result.Message = exception.Message;
                //exception.Redirect();
            }

            return result;
        }

        public async Task<BaseResponse<CentralPayLogDto>> CreatePaymentRequest(int invoiceId)
        {
            var result = new BaseResponse<CentralPayLogDto>();

            try
            {
                var response = await _http.GetFromJsonAsync<BaseResponse<CentralPayLogDto>>($"api/Billings/CreateInvoicePaymentRequest?invoiceId={invoiceId}");

                if (response != null && response.Success)
                {
                    if (response.Result != null)
                    {
                        result = response;
                    }
                }
            }
            catch (Exception exception)
            {
                result.Success = false;
                result.Message = exception.Message;
                //exception.Redirect();
            }

            return result;
        }

        public async Task<BaseResponse<CentralPayLogDto>> VerifyPaymentRequest(string requestId)
        {
            var result = new BaseResponse<CentralPayLogDto>();

            try
            {
                var response = await _http.GetFromJsonAsync<BaseResponse<CentralPayLogDto>>($"api/Billings/VerifyPaymentRequest?requestId={requestId}");

                if (response != null && response.Success)
                {
                    if (response.Result != null)
                    {
                        result = response;
                    }
                }
            }
            catch (Exception exception)
            {
                result.Success = false;
                result.Message = exception.Message;
                //exception.Redirect();
            }

            return result;
        }
    }

	public interface IBillingService
	{
		Task<BaseResponse<List<BillInvoiceDto>>> GetMyInvoices(int pageIdex, int pageSize);

		Task<BaseResponse<BillInvoiceDto>> GetInvoice(int invoiceId);

        Task<BaseResponse<CentralPayLogDto>> CreatePaymentRequest(int invoiceId);

        Task<BaseResponse<CentralPayLogDto>> VerifyPaymentRequest(string requestId);
    }
}
