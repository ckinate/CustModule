using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;

public class CustomerDto : BaseDto
{
    public string? Code { get; set; }
    public string? CustomCode { get; set; }

    public string? Name { get; set; }
    public string? RegistrationCertificateNumber { get; set; }
    public DateTime? IncorporationDate { get; set; }
    public string? RegisterAddress1 { get; set; }
    public string? RegisterAddress2 { get; set; }

    public string TaxIdentificationNumber { get; set; }

    public string? Country { get; set; }
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

    public int? ChildInstitutionTypeId { get; set; }
    public string? ChildInstitutionTypeName { get; set; }

    public string? InstitutionCode { get; set; }

    public StaffSize StaffSize { get; set; }

    public DateTime RegistrationDate { get; set; }

    public bool IsPublic { get; set; }

	public bool IncludeLocalAccount { get; set; }

	public string? LoginId { get; set; }

	public bool IsLock { get; set; }

	public OnboardingStatus Status { get; set; }
	public string? StatusDisplay { get; set; }

	public bool RequireSync { get; set; }

	public bool DueDiligenceCompleted { get; set; }

	public bool IsCorrespondentBank { get; set; }
	public bool HasSubsidiary { get; set; }
	public bool IsSubsidiary { get; set; }
	public int? ParentId { get; set; }
	public string? ParentName { get; set; } = string.Empty;
	public string? ParentCode { get; set; } = string.Empty;
	public string? ParentInstitutionCode { get; set; } = string.Empty;

	public List<ContactPersonDto> CustomerContactPersons { get; set; } = default!;
	public List<ContactChannelDto> CustomerContactChannels { get; set; } = default!;
	public List<CustomerAccountDto> CustomerAccounts { get; set; } = default!;
	public List<CustomerSignatoryDto> CustomerSignatories { get; set; } = default!;
	public List<DocumentDto> CustomerDocuments { get; set; } = default!;

}

public class ContactPersonDto
{
	public int? Id { get; set; }
	public string FirstName { get; set; } = string.Empty;
	public string MiddleName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string Name { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;

	public string MobilePhoneCallCode { get; set; } = string.Empty;
	public string MobilePhoneNumber { get; set; } = string.Empty;
	public string Designation { get; set; } = string.Empty;
	public bool Default { get; set; }
}

public class ContactChannelDto
{
	public ChannelType Type { get; set; } = ChannelType.Email;
	public string Email { get; set; } = string.Empty;

	public string MobilePhoneCallCode { get; set; } = string.Empty;
	public string MobilePhoneNumber { get; set; } = string.Empty;
}

public class CustomerAccountDto
{
	public int? Id { get; set; }
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

public class CustomerSignatoryDto
{
	public string FirstName { get; set; } = string.Empty;
	public string MiddleName { get; set; } = string.Empty;
	public string LastName { get; set; } = string.Empty;
	public string? Name { get; set; }
    public string? Email { get; set; }
    public string? MobileNumberCallCode { get; set; }
    public string? MobileNumber { get; set; }
}

public class DocumentDto
{
	public int Id { get; set; }

	public int DocumentTypeId { get; set; }
	public string DocumentTypeName { get; set; }

	public string Title { get; set; } = string.Empty;
    public virtual DateTime? IssueDate { get; set; }
    public virtual bool HasExpiryDate { get; set; }
    public virtual DateTime? ExpiryDate { get; set; }

    public byte[] FileData { get; set; } = default!;
	public string ContentType { get; set; } = string.Empty;
	public long Size { get; set; }
	public string Location { get; set; } = string.Empty;

    public bool ViewableByCustomer { get; set; }

    public bool RequireCustomerSignature { get; set; }

    public bool CustomerHaveSigned { get; set; }

    public bool Completed { get; set; }

    public string? Remark { get; set; }
}

public class CustomerExportDto : BaseDto
{
    public string? Code { get; set; }
    public string? CustomCode { get; set; }

    public string? Name { get; set; }
    public string? RegistrationCertificateNumber { get; set; }
    public DateTime? IncorporationDate { get; set; }
    public string? RegisterAddress1 { get; set; }
    public string? RegisterAddress2 { get; set; }

    public string TaxIdentificationNumber { get; set; }

    public string? Country { get; set; }
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

    public int? ChildInstitutionTypeId { get; set; }
    public string? ChildInstitutionTypeName { get; set; }

    public string? InstitutionCode { get; set; }

    public StaffSize StaffSize { get; set; }

    public DateTime RegistrationDate { get; set; }

    public bool IsPublic { get; set; }

    public bool IncludeLocalAccount { get; set; }

    public string? LoginId { get; set; }

    public bool IsLock { get; set; }

    public OnboardingStatus Status { get; set; }
    public string? StatusDisplay { get; set; }

    public bool RequireSync { get; set; }

    public bool DueDiligenceCompleted { get; set; }

    public bool IsCorrespondentBank { get; set; }
    public bool HasSubsidiary { get; set; }
    public bool IsSubsidiary { get; set; }
    public int? ParentId { get; set; }
    public string? ParentName { get; set; } = string.Empty;
    public string? ParentCode { get; set; } = string.Empty;
    public string? ParentInstitutionCode { get; set; } = string.Empty;

    public string? SettlementBankCode { get; set; } = string.Empty;
    public string? SettlementBankName { get; set; } = string.Empty;

}
