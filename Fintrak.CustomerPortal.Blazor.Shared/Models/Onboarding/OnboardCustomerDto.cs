using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;

public class OnboardCustomerDto
{
    public int Id { get; set; }

	public string Code { get; set; } = string.Empty;

    public string CustomCode { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

	public string RegistrationCertificateNumber { get; set; } = string.Empty;

	public DateTime IncorporationDate { get; set; } = DateTime.Now;

	public string RegisterAddress1 { get; set; } = string.Empty;

    public string RegisterAddress2 { get; set; } = string.Empty;

    public string? TaxIdentificationNumber { get; set; }

	public int? CountryId { get; set; }
	public string Country { get; set; } = string.Empty;

	public int? StateId { get; set; }
	public string State { get; set; } = string.Empty;

	public string City { get; set; } = string.Empty;

    public string? BusinessNature { get; set; } = string.Empty;

    public string? OfficePhoneCallCode { get; set; }
	public string? OfficePhoneNumber { get; set; }

	public string? MobilePhoneCallCode { get; set; }
	public string? MobilePhoneNumber { get; set; }

	public string? Email { get; set; }
	public string? Fax { get; set; }
	public string? Website { get; set; }

	public int? SectorId { get; set; }
	public string? SectorName { get; set; }

	public int? IndustryId { get; set; }
	public string IndustryName { get; set; } = string.Empty;

    public int? InstitutionTypeId { get; set; }
    public string? InstitutionTypeName { get; set; }

	public bool HasChildInstitutionType { get; set; }
	public int? ChildInstitutionTypeId { get; set; }
    public string? ChildInstitutionTypeName { get; set; }

    public string? InstitutionCode { get; set; }

    public StaffSize StaffSize { get; set; }

    public bool IsPublic { get; set; }

	public string? LogoLocation { get; set; }

	public byte[]? LogoFileData { get; set; } = default!;
	public string? LogoContentType { get; set; } = string.Empty;
	public long? LogoSize { get; set; }
	public string? LogoSelectedFileName { get; set; } = string.Empty;

	public bool IsLock { get; set; }

	public bool DueDiligenceCompleted { get; set; }

	public bool CanUpdate { get; set; }

	public OnboardingStatus Status { get; set; }

	public string? SettlementBankCode { get; set; } = string.Empty;
	public string? SettlementBankName { get; set; } = string.Empty;

	public bool IsCorrespondentBank { get; set; }
	public bool HasSubsidiary { get; set; }
	public bool IsSubsidiary { get; set; }
	public int? ParentId { get; set; }
	public string? ParentName { get; set; } = string.Empty;
	public string? ParentCode { get; set; } = string.Empty;
	public string? ParentInstitutionCode { get; set; } = string.Empty;

	public List<UpsertContactPersonDto> ContactPersons { get; set; } = new();
	public List<UpsertContactChannelDto> ContactChannels { get; set; } = new();
	public List<UpsertBankAccountDto> BankAccounts { get; set; } = new();
	public List<UpsertSignatoryDto> Signatories { get; set; } = new();
	public List<UpsertDocumentDto> Documents { get; set; } = new();

	public List<UpsertCustomFieldDto> CustomFields { get; set; } = new();
}

public class BasicCustomerDto
{
	public int Id { get; set; }

	public string Code { get; set; } = string.Empty;

	public string CustomCode { get; set; } = string.Empty;

	public string Name { get; set; } = string.Empty;

	public string RegistrationCertificateNumber { get; set; } = string.Empty;

	public DateTime IncorporationDate { get; set; } = DateTime.Now;

	public string RegisterAddress1 { get; set; } = string.Empty;

	public string RegisterAddress2 { get; set; } = string.Empty;

	public string? TaxIdentificationNumber { get; set; }

	public int? CountryId { get; set; }
	public string Country { get; set; } = string.Empty;

	public int? StateId { get; set; }
	public string State { get; set; } = string.Empty;

	public string City { get; set; } = string.Empty;

	public string? BusinessNature { get; set; } = string.Empty;

	public string? OfficePhoneCallCode { get; set; }
	public string? OfficePhoneNumber { get; set; }

	public string? MobilePhoneCallCode { get; set; }
	public string? MobilePhoneNumber { get; set; }

	public string? Email { get; set; }
	public string? Fax { get; set; }
	public string? Website { get; set; }

	public int? SectorId { get; set; }
	public string? SectorName { get; set; }

	public int? IndustryId { get; set; }
	public string IndustryName { get; set; } = string.Empty;

	public int? InstitutionTypeId { get; set; }
	public string? InstitutionTypeName { get; set; }

	public bool HasChildInstitutionType { get; set; }
	public int? ChildInstitutionTypeId { get; set; }
	public string? ChildInstitutionTypeName { get; set; }

	public string? InstitutionCode { get; set; }

	public StaffSize StaffSize { get; set; }

	public bool IsPublic { get; set; }

    public string LogoLocation { get; set; }

    public bool IsLock { get; set; }

	public bool DueDiligenceCompleted { get; set; }

	public bool CanUpdate { get; set; }

	public virtual CustomerStage? Stage { get; set; }

	public OnboardingStatus Status { get; set; }

	public string SettlementBankCode { get; set; }
	public string SettlementBankName { get; set; }

	public bool IsCorrespondentBank { get; set; }
	public bool HasSubsidiary { get; set; }
	public bool IsSubsidiary { get; set; }
	public CustomerDto Parent { get; set; }

}

public class UpsertContactPersonDto
{
	public string? FirstName { get; set; }
	public string? MiddleName { get; set; }
	public string? LastName { get; set; }

	public string Name { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;

	public string MobilePhoneCallCode { get; set; } = string.Empty;
	public string MobilePhoneNumber { get; set; } = string.Empty;

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

	public static string GetFullName(string firstName, string middleName, string lastName)
	{
		if (string.IsNullOrEmpty(middleName))
			return $"{firstName} {lastName}";
		else
			return $"{firstName} {middleName} {lastName}";
	}
}

public class UpsertContactChannelDto
{
	public ChannelType Type { get; set; } = ChannelType.Email;
	public string Email { get; set; } = string.Empty;

	public string MobilePhoneCallCode { get; set; } = string.Empty;
	public string MobilePhoneNumber { get; set; } = string.Empty;
}

public class UpsertBankAccountDto
{
    public AccountType AccountType { get; set; }
    public string BankName { get; set; } = string.Empty;
	public string BankCode { get; set; } = string.Empty;
	public string Country { get; set; } = string.Empty;
	public string? BankAddress { get; set; } = string.Empty;
	public string AccountName { get; set; } = string.Empty;
	public string AccountNumber { get; set; } = string.Empty;
	public bool IsLocalAccount { get; set; }
	public bool Validated { get; set; }
}

public class UpsertSignatoryDto
{
	public string? FirstName { get; set; }
	public string? MiddleName { get; set; }
	public string? LastName { get; set; }

	public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobilePhoneCallCode { get; set; } = string.Empty;
    public string MobilePhoneNumber { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;

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

	public static string GetFullName(string firstName, string middleName, string lastName)
	{
		if (string.IsNullOrEmpty(middleName))
			return $"{firstName} {lastName}";
		else
			return $"{firstName} {middleName} {lastName}";
	}
}

public class UpsertDocumentDto
{
    public int DocumentTypeId { get; set; } 
    public string DocumentTypeName { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;
	public byte[]? FileData { get; set; } = default!;
	public string ContentType { get; set; } = string.Empty;
	public long Size { get; set; }

	public bool HasIssueDate { get; set; }
	public DateTime? IssueDate { get; set; } = default!;
	public bool HasExpiryDate { get; set; }
	public DateTime? ExpiryDate { get; set; } = default!;

	public int? DocumentId { get; set; }
	public string LocationUrl { get; set; } = string.Empty;

    public bool CanUpdate { get; set; }
    public bool ViewableByCustomer { get; set; }
    public bool RequireCustomerSignature { get; set; }
    public bool CustomerHaveSigned { get; set; }
    public bool Completed { get; set; }
    public string Remark { get; set; } = string.Empty;

}



