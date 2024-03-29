namespace Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct
{
	public class CustomFieldModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public bool IsCompulsory { get; set; }
	}

	public class CustomFieldDto
	{
		public int CustomFieldId { get; set; }

		public string CustomField { get; set; } = default!;

		public bool IsCompulsory { get; set; }

		public string Response { get; set; } = default!;
	}

	public class UpsertCustomFieldDto
	{
		public int CustomFieldId { get; set; }

		public string CustomField { get; set; } = default!;

		public bool IsCompulsory { get; set; }

		public string Response { get; set; } = default!;
	}
}
