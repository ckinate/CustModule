using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Billings;

namespace Fintrak.CustomerPortal.Application.Common.Interfaces
{
	public interface ICustomerIntegrationService
	{
		Task<BaseResponse<List<LookupModel>>> GetCountryLookUp(bool includeAll = false);

		Task<BaseResponse<List<LookupModel>>> GetStateLookUp(int? countryId, bool includeAll = false);

		Task<BaseResponse<List<LookupModel>>> GetSectorLookUp(bool includeAll = false);

		Task<BaseResponse<List<LookupModel>>> GetIndustryLookUp(int? sectorId = null, bool includeAll = false);

		Task<BaseResponse<List<LookupModel>>> GetSubsidiaryHeadsLookUp(string customerCode, bool includeAll = false);

		Task<BaseResponse<List<LookupModel>>> GetInstitutionTypeLookUp(int? parentId, bool? parentOnly = false, bool includeAll = false);

		Task<BaseResponse<List<LookupModel>>> GetInstitutionTypeDocumentLookUp(int? institutionTypeId, bool includeAll = false);

		Task<BaseResponse<List<LookupModel>>> GetProductLookUp(string customerCode, bool includeAll = false);

		Task<BaseResponse<List<LookupModel>>> GetProductDocumentLookUp(int productId, string customerCode, bool includeAll = false);

		Task<BaseResponse<List<CustomFieldDto>>> GetProductCustomFields(int productId);

		Task<BaseResponse<string>> GetCustomerNotificationEmail();

		Task<BaseResponse<List<CustomFieldResponseDto>>> GetCustomerCustomFields(string customerCode = "");

        Task<BaseResponse<RecentCustomerOnboardingTrackerDto>> GetRecentTrackers(string customerCode);

		Task<BaseResponse<AcceptOnboadingDataResponseDto>> TestCustomerDataAcceptance(OnboardCustomerDto onboardCustomer);

		Task<BaseResponse<List<BillInvoiceDto>>> GetInvoices(string customerCode, int pageIndex, int pageSize);

		Task<BaseResponse<BillInvoiceDto>> GetInvoice(int invoiceId);

		Task<BaseResponse<CentralPayLogDto>> CreatePaymentRequest(int invoiceId);

		Task<BaseResponse<CentralPayLogDto>> VerifyPaymentRequest(string requestId);

		Task<BaseResponse<bool>> ProcessDocusignRequest(DocusignWebhookCompletedModel model);


		Task<BaseResponse<long>> GetSize(int? documentTypeId);
		Task<BaseResponse<bool>> SubmitPaymentReceipt(PortalInvoicePaymentReceiptDto model);
		Task<BaseResponse<List<CustomerOnboardingDocumentDto>>> GetCustomerLegalDocuments(string customerCode);
		Task<BaseResponse<bool>> UploadSignedDocuments(string customerCode, List<CustomerOnboardingDocumentDto> documents);
		Task<BaseResponse<bool>> SubmitSignedDocuments(string customerCode);
		Task<BaseResponse<bool>> UploadAndSubmitSignedDocuments(string customerCode, List<CustomerOnboardingDocumentDto> documents);
	}
}
