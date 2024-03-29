namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding
{

	public class SignedDocuments
	{
        public List<CustomerOnboardingDocumentDto> Documents { get; set; }
    }

	public class CustomerOnboardingDocumentDto
	{
		public int? Id { get; set; }
		public string CustomerCode { get; set; }
		public int PartnerDocumentTypeId { get; set; }
		public string Title { get; set; }
		public FileUploadDto FileUploadData { get; set; }
		public bool HasIssueDate { get; set; }
		public DateTime? IssueDate { get; set; }
		public bool HasExpiryDate { get; set; }
		public DateTime? ExpiryDate { get; set; }
		public virtual string Location { get; set; }

        public string SelectedFileName { get; set; }
		public bool FileUploaded { get; set; }
	}

	public class FileUploadDto
	{
		public string FileName { get; set; }

		public virtual byte[] FileData { get; set; }

		public virtual string FileExtensionType { get; set; }

		public long FileSize { get; set; }
	}
}
