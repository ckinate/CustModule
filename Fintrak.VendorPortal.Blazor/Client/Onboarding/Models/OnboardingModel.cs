using Fintrak.VendorPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.VendorPortal.Blazor.Client.Onboarding.Models
{
	public class OnboardingModel
	{
		public int CurrentStep { get; set; } = 1;

        public OfficialInformationModel OfficialInformation { get; set; } = new();

		public ContactPersonsModel ContactPersons { get; set; } = new();

		public ContactChannelsModel ContactChannels { get; set; } = new();

		public LocalBankModel LocalBank { get; set; } = new();

		public ForeignBanksModel ForeignBanks { get; set; } = new();

		public QuestionnairesModel Questionnaires { get; set; } = new();

		public DocumentsModel Documents { get; set; } = new();

		public bool CanUpdate { get; set; }
	}

	public class OfficialInformationModel
	{
		public string CompanyName { get; set; } = string.Empty;

		public string RegistrationCertificateNumber { get; set; } = string.Empty;

		public DateTime? IncorporationDate { get; set; } = DateTime.Now;

		public string RegisterAddress { get; set; } = string.Empty;

		public string? TaxIdentificationNumber { get; set; }

		public string Country { get; set; } = string.Empty;

		public string? OfficePhoneCallCode { get; set; }
		public string? OfficePhoneIsoCode { get; set; }
		public string? OfficePhoneNumber { get; set; }
		public string? OfficePhoneNumberDisplay 
		{
			get
			{
				return $"({OfficePhoneCallCode}) {OfficePhoneNumber}";
			}  
		}

		public string? MobilePhoneCallCode { get; set; }
		public string? MobilePhoneIsoCode { get; set; }
		public string? MobilePhoneNumber { get; set; }
		public string? MobilePhoneNumberDisplay
		{
			get
			{
				return $"({MobilePhoneCallCode}) {MobilePhoneNumber}";
			}
		}

		public string? Email { get; set; }
		public string? Fax { get; set; }
		public string? Website { get; set; }

		public int? CategoryId { get; set; }
		public string? CategoryName { get; set; }

		public int?[] SubCategoryIds { get; set; } = new int?[] { };
		public string SubCategoryNames { get; set; } = string.Empty;
		public bool UseForeignAccount { get; set; }
		public bool IsPublic { get; set; }
		public bool IncludeLocalAccount { get; set; }
        public bool DueDiligenceCompleted { get; set; }

        public OnboardingStatus Status { get; set; }
    }

	public class ContactPersonsModel
	{
		public List<ContactPersonModel> ContactPersons { get; set; } = new();
	}

	public class ContactPersonModel
	{
		public Guid FormId { get; set; } = default!;
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;

		public string MobilePhoneCallCode { get; set; } = string.Empty;
		public string MobilePhoneIsoCode { get; set; } = string.Empty;
		public string MobilePhoneNumber { get; set; } = string.Empty;
		public string? MobilePhoneNumberDisplay
		{
			get
			{
				return $"({MobilePhoneCallCode}) {MobilePhoneNumber}";
			}
		}
		public string Designation { get; set; } = string.Empty;
		public bool Default { get; set; }
	}

	public class ContactChannelsModel
	{
		public List<ContactChannelModel> ContactChannels { get; set; } = new();
	}

	public class ContactChannelModel
	{
		public Guid FormId { get; set; } = default!;
		public ChannelType Type { get; set; } = ChannelType.Email;
		public string Email { get; set; } = string.Empty;

		public string MobilePhoneCallCode { get; set; } = string.Empty;
		public string MobilePhoneIsoCode { get; set; } = string.Empty;
		public string MobilePhoneNumber { get; set; } = string.Empty;
		public string? MobilePhoneNumberDisplay
		{
			get
			{
				return $"({MobilePhoneCallCode}) {MobilePhoneNumber}";
			}
		}
	}

	public class LocalBankModel
	{
		public string BankName { get; set; } = string.Empty;

		public string BankCode { get; set; } = string.Empty;

		public string? BankAddress { get; set; } = string.Empty;

		public string AccountName { get; set; } = string.Empty;

		public string AccountNumber { get; set; } = string.Empty;

		public bool Validated { get; set; }

		public bool IsCompulsory { get; set; }
	}

	public class ForeignBanksModel
	{
		public List<ForeignBankModel> ForeignBanks { get; set; } = new();
	}

	public class ForeignBankModel
	{
		public Guid FormId { get; set; } = default!;
		public string BankName { get; set; } = string.Empty;

		public string BankCode { get; set; } = string.Empty;

		public string Country { get; set; } = string.Empty;

		public string? BankAddress { get; set; } = string.Empty;

		public string AccountName { get; set; } = string.Empty;

		public string AccountNumber { get; set; } = string.Empty;
	}

	public class QuestionnairesModel
	{
		public List<QuestionnaireModel> Questionnaires { get; set; } = new();
	}

	public class QuestionnaireModel
	{
		public int QuestionId { get; set; }

		public string Question { get; set; } = string.Empty;

		public string Response { get; set; } = string.Empty;

		public bool Compulsory { get; set; }
	}

	public class DocumentsModel
	{
		public List<DocumentModel> Documents { get; set; } = new();
	}

	public class DocumentModel
	{
		public Guid FormId { get; set; } = default!;
        public string SelectedFileName { get; set; } = string.Empty;


        public string Title { get; set; } = string.Empty;

		public byte[] FileData { get; set; } = default!;

		public string ContentType { get; set; } = string.Empty;

		public long Size { get; set; }

		public int? DocumentId { get; set; }
		public string LocationUrl { get; set; } = string.Empty;

	}
}
