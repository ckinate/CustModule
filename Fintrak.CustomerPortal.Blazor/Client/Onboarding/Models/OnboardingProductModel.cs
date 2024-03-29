using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models
{
	public class OnboardingProductModel
	{
		public int? Id { get; set; }

		public string CustomerCode { get; set; }

		public string CustomerName { get; set; }

		public int ProductId { get; set; }

		public string ProductCode { get; set; }

		public string ProductName { get; set; }

		public int ContactPersonId { get; set; }

		public OperationMode OperationMode { get; set; }

		public int AccountId { get; set; }

		public string Reason { get; set; }

		public string Website { get; set; }

		public List<DocumentModel> Documents { get; set; } = new();

		public List<UpsertCustomFieldDto> AdditionalInformations { get; set; } = new();
	}

	public class UpsertCustomFieldDto
	{
		public int CustomFieldId { get; set; }

		public string CustomField { get; set; }

		public bool IsCompulsory { get; set; }

		public string Response { get; set; }
	}
}
