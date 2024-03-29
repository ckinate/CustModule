using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct
{
	public class CustomerProductDto
	{
		public int Id { get; set; }

		public CustomerDto Customer { get; set; }

		public int ProductId { get; set; }
		public string ProductName { get; set; } = default!;
		public string ProductCode { get; set; } = default!;

		public ContactPersonDto CustomerContactPerson { get; set; }

		public OperationMode OperationMode { get; set; }

		public CustomerAccountDto CustomerAccount { get; set; }

		public string Reason { get; set; } = default!;

		public virtual string Website { get; set; } = default!;

		public bool CanUpdate { get; set; }

		//Draft,Submitted,Completed
		public OnboardingProductStatus Status { get; set; }
		public string? StatusDisplay { get; set; }

		public IList<CustomFieldDto> CustomerProductCustomFields { get; private set; } = new List<CustomFieldDto>();
		public IList<DocumentDto> CustomerProductDocuments { get; private set; } = new List<DocumentDto>();
	}
}
