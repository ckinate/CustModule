using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models
{
	public class DashboardModel
	{
        public List<OnboardingProductModel> Products { get; set; } = new();
	}

	public class OnboardingModel
	{
		public int CurrentStep { get; set; } = 1;
        public bool HasSettlementBank { get; set; }
        public bool HasFeeAccounts { get; set; }

        public OfficialInformationModel OfficialInformation { get; set; } = new();

		public ContactPersonsModel ContactPersons { get; set; } = new();

		public ContactChannelsModel ContactChannels { get; set; } = new();

		public BankAccountsModel FeeAccounts { get; set; } = new();

		public BankAccountsModel CommissionAccounts { get; set; } = new();

		public SignatoriesModel Signatories { get; set; } = new();

		public DocumentsModel Documents { get; set; } = new();

		public CustomFieldsModel CustomFields { get; set; } = new();

		public bool CanUpdate { get; set; }
	}

	public class CustomerModel
	{
		public string Code { get; set; } = string.Empty;

		public string CustomCode { get; set; } = string.Empty;

		public string CompanyName { get; set; } = string.Empty;

		public string RegistrationCertificateNumber { get; set; } = string.Empty;

		public DateTime? IncorporationDate { get; set; } = DateTime.Now;

		public string RegisterAddress1 { get; set; } = string.Empty;

		public string RegisterAddress2 { get; set; } = string.Empty;

		public string? TaxIdentificationNumber { get; set; }

		public int? CountryId { get; set; }
		public string Country { get; set; } = string.Empty;

		public int? StateId { get; set; }
		public string State { get; set; } = string.Empty;

		public string City { get; set; } = string.Empty;

		public string BusinessNature { get; set; } = string.Empty;

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

		public int? SectorId { get; set; }
		public string? SectorName { get; set; }

		public int? IndustryId { get; set; }
		public string IndustryName { get; set; }

		public string? LogoLocation { get; set; }

		public int? InstitutionTypeId { get; set; }
		public string InstitutionTypeName { get; set; }

		public bool HasChildInstitutionType { get; set; }
		public int? ChildInstitutionTypeId { get; set; }
		public string ChildInstitutionTypeName { get; set; }

		public string InstitutionCode { get; set; }

		public StaffSize StaffSize { get; set; }

		public bool IsPublic { get; set; }

		public bool DueDiligenceCompleted { get; set; }

		public OnboardingStatus Status { get; set; }

		public string? SettlementBankCode { get; set; }
		public string? SettlementBankName { get; set; }

		public bool IsCorrespondentBank { get; set; }
		public bool HasSubsidiary { get; set; }
		public bool IsSubsidiary { get; set; }
		public int? ParentId { get; set; }

		public string? ParentName { get; set; }
		public string? ParentCode { get; set; }
		public string? ParentInstitutionCode { get; set; }
	}

	public class OfficialInformationModel
	{
		public string Code { get; set; } = string.Empty;

		public string CustomCode { get; set; } = string.Empty;

		public string CompanyName { get; set; } = string.Empty;

		public string RegistrationCertificateNumber { get; set; } = string.Empty;

		public DateTime? IncorporationDate { get; set; } = DateTime.Now;

		public string RegisterAddress1 { get; set; } = string.Empty;

        public string RegisterAddress2 { get; set; } = string.Empty;

        public string? TaxIdentificationNumber { get; set; }

		public int? CountryId { get; set; }
		public string Country { get; set; } = string.Empty;

		public int? StateId { get; set; }
		public string State { get; set; } = string.Empty;

		public string City { get; set; } = string.Empty;

        public string BusinessNature { get; set; } = string.Empty;

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

		public int? SectorId { get; set; }
		public string? SectorName { get; set; }

		public int? IndustryId { get; set; } 
		public string IndustryName { get; set; }

		public string? LogoLocation { get; set; }
		public byte[]? LogoFileData { get; set; } = default!;
		public string? LogoContentType { get; set; } = string.Empty;
		public long? LogoSize { get; set; }
		public string? LogoSelectedFileName { get; set; } = string.Empty;

		public int? InstitutionTypeId { get; set; }
        public string InstitutionTypeName { get; set; }

		public bool HasChildInstitutionType { get; set; }
		public int? ChildInstitutionTypeId { get; set; }
        public string ChildInstitutionTypeName { get; set; }

        public string InstitutionCode { get; set; }

        public StaffSize StaffSize { get; set; }

        public bool IsPublic { get; set; }

        public bool DueDiligenceCompleted { get; set; }

        public OnboardingStatus Status { get; set; }

		public string? SettlementBankCode { get; set; }
		public string? SettlementBankName { get; set; }

		public bool IsCorrespondentBank { get; set; }
		public bool HasSubsidiary { get; set; }
		public bool IsSubsidiary { get; set; }
		public int? ParentId { get; set; }
		public string? ParentName { get; set; }
		public string? ParentCode { get; set; }
		public string? ParentInstitutionCode { get; set; }
	}

	public class ContactPersonsModel
	{
		public List<ContactPersonModel> ContactPersons { get; set; } = new();
	}

	public class ContactPersonModel
	{
		public Guid FormId { get; set; } = default!;
		public string FirstName { get; set; } = string.Empty;
		public string MiddleName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
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

		public string FullName
		{
			get
			{
				if (string.IsNullOrEmpty(MiddleName))
					Name = $"{FirstName} {LastName}";
				else
					Name = $"{FirstName} {MiddleName} {LastName}";

				return Name;
			}
		}
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

	public class BankAccountsModel
	{
		public List<BankAccountModel> BankAccounts { get; set; } = new();
	}

	public class BankAccountModel
	{
		public Guid FormId { get; set; } = default!;

        public AccountType AccountType { get; set; }

        public string BankName { get; set; } = string.Empty;

		public string BankCode { get; set; } = string.Empty;

		public string Country { get; set; } = string.Empty;

		public string? BankAddress { get; set; } = string.Empty;

		public string AccountName { get; set; } = string.Empty;

		public string AccountNumber { get; set; } = string.Empty;

		public bool Validated { get; set; } = false;
	}

	public class SignatoriesModel
	{
		public List<SignatoryModel> Signatories { get; set; } = new();
	}

	public class SignatoryModel
	{
		public Guid FormId { get; set; } = default!;

		public string FirstName { get; set; } = string.Empty;
		public string MiddleName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public string Name { get; set; }

        public string Email { get; set; }

        public string MobileNumberCallCode { get; set; }
		public string MobileNumberIsoCode { get; set; } = string.Empty;
		public string MobileNumber { get; set; }
		public string? MobilePhoneNumberDisplay
		{
			get
			{
				return $"({MobileNumberCallCode}) {MobileNumber}";
			}
		}
		public string Designation { get; set; }

		public string FullName
		{
			get
			{
				if (string.IsNullOrEmpty(MiddleName))
					Name = $"{FirstName} {LastName}";
				else
					Name = $"{FirstName} {MiddleName} {LastName}";

				return Name;
			}
		}
	}

	public class DocumentsModel
	{
		public List<DocumentModel> Documents { get; set; } = new();
	}

	public class DocumentModel
	{
		public Guid FormId { get; set; } = default!;
        public string SelectedFileName { get; set; } = string.Empty;

        public int? DocumentTypeId { get; set; }
		public string DocumentTypeName { get; set; } = string.Empty;
		public string Title { get; set; } = string.Empty;

		public bool HasIssueDate { get; set; }
		public DateTime? IssueDate { get; set; }
		public bool HasExpiryDate { get; set; }
		public DateTime? ExpiryDate { get; set; }

		public byte[] FileData { get; set; } = default!;
		public string ContentType { get; set; } = string.Empty;
		public long Size { get; set; }

		public int? DocumentId { get; set; }
		public string LocationUrl { get; set; } = string.Empty;

	}

	public class CustomFieldsModel
	{
		public List<CustomFieldModel> CustomFields { get; set; } = new();
	}

	public class CustomFieldModel
	{
		public int FieldId { get; set; }

		public string Field { get; set; } = string.Empty;

		public string Response { get; set; } = string.Empty;

		public bool Compulsory { get; set; }
	}
}
