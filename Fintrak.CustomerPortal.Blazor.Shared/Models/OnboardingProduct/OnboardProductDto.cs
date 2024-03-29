using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct
{
	public class OnboardProductDto
	{
		public int? Id { get; set; }

		//public CustomerDto Customer { get; set; }

		public string CustomerCode { get; set; }

		public string CustomerName { get; set; }

		public int ProductId { get; set; }
		public string ProductName { get; set; } = default!;
		public string ProductCode { get; set; } = default!;

		public int ContactPersonId { get; set; }
		public string ContactPersonName { get; set; }

		public OperationMode OperationMode { get; set; }

		public int AccountId { get; set; }
		public string AccountNumber { get; set; }

		public string Reason { get; set; } = default!;
		public string Remark { get; set; } = default!;
		public virtual string Website { get; set; } = default!;

		public bool CanUpdate { get; set; }

		//Draft,Submitted,Completed
		public OnboardingProductStatus Status { get; set; }

		public List<UpsertCustomFieldDto> CustomerProductCustomFields { get; set; } = new List<UpsertCustomFieldDto>();
		public List<UpsertDocumentDto> CustomerProductDocuments { get; set; } = new List<UpsertDocumentDto>();
	}
}
