using Fintrak.CustomerPortal.Domain.Enums;

namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class Customer : BaseAuditableEntity
	{
		public string? Code { get; set; }
        public string? CustomCode { get; set; }

        public string? Name { get; set; }
		public string? RegistrationCertificateNumber { get; set; }
		public DateTime? IncorporationDate { get; set; }
		public string? RegisterAddress1 { get; set; }
        public string? RegisterAddress2 { get; set; }

        public string TaxIdentificationNumber { get; set; }

        public int? CountryId { get; set; }
        public string? Country { get; set; }

        public int? StateId { get; set; }
        public string? State { get; set; }

        public string? City { get; set; }
        public string? BusinessNature { get; set; }

        public string? OfficePhoneCallCode { get; set; }
		public string? OfficePhoneNumber { get; set; }

		public string? MobilePhoneCallCode { get; set; }
		public string? MobilePhoneNumber { get; set; }

		public string? Email { get; set; }
		public string Fax { get; set; }
		public string Website { get; set; }

        public string LogoLocation { get; set; }

        public int? SectorId { get; set; }
		public string? SectorName { get; set; }

        public int? IndustryId { get; set; }
        public string? IndustryName { get; set; }

        public int? InstitutionTypeId { get; set; }
        public string? InstitutionTypeName { get; set; }

		public bool HasChildInstitutionType { get; set; }
		public int? ChildInstitutionTypeId { get; set; }
        public string? ChildInstitutionTypeName { get; set; }

        public string? InstitutionCode { get; set; }


        public bool IsCorrespondentBank { get; set; }
		public bool HasSubsidiary { get; set; }
		public bool IsSubsidiary { get; set; }

        public int? ParentId { get; set; }
		public Customer Parent { get; set; }

        public string ParentCode { get; set; }
		public string ParentName { get; set; }

		public StaffSize StaffSize { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsPublic { get; set; }

		public string? LoginId { get; set; }

		public string? SettlementBankCode { get; set; }

		public string? SettlementBankName { get; set; }

		public bool IsLock { get; set; }

		public virtual CustomerStage? Stage { get; set; }

		public OnboardingStatus Status { get; set; }

		public bool RequireSync { get; set; }

		public bool DueDiligenceCompleted { get; set; }

		public IList<CustomerContactPerson> CustomerContactPersons { get; private set; } = new List<CustomerContactPerson>();
		public IList<CustomerContactChannel> CustomerContactChannels { get; private set; } = new List<CustomerContactChannel>();
        public IList<CustomerSignatory> CustomerSignatories { get; private set; } = new List<CustomerSignatory>();
        public IList<CustomerAccount> CustomerAccounts { get; private set; } = new List<CustomerAccount>();
		public IList<CustomerDocument> CustomerDocuments { get; private set; } = new List<CustomerDocument>();
        public IList<CustomerCustomField> CustomertCustomFields { get; private set; } = new List<CustomerCustomField>();

        public IList<CustomerProduct> CustomerProducts { get; private set; } = new List<CustomerProduct>();
	}
}
