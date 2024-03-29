using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Utilities;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.FileIO;
using MimeKit;
using MimeDetective;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

public record class GetCustomerDetailByIdQuery(int CustomerId, string Hash) : IRequest<BaseResponse<OnboardCustomerDto>>;

public class GetCustomerDetailByIdQueryHandler : IRequestHandler<GetCustomerDetailByIdQuery, BaseResponse<OnboardCustomerDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public GetCustomerDetailByIdQueryHandler(IApplicationDbContext context, IMapper mapper, IConfiguration configuration)
	{
		_context = context;
		_mapper = mapper;
		_configuration = configuration;
    }

	public async Task<BaseResponse<OnboardCustomerDto>> Handle(GetCustomerDetailByIdQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<OnboardCustomerDto>();

		//if (!ValidateQuery(request))
		//{
		//	throw new Exception("[GetCustomerDetailByIdQuery] - Request parameters not properly formated.");
		//}

		var customer = await _context.Customers
			.Include(c=> c.CustomerContactPersons)
			.Include(c => c.CustomerContactChannels)
			.Include(c => c.CustomerAccounts)
			.Include(c => c.CustomerSignatories)
			.Include(c => c.CustomerDocuments)
            .Include(c => c.CustomertCustomFields)
			.Include(c => c.Parent)
			.FirstOrDefaultAsync(c => c.Id == request.CustomerId);

		if (customer == null)
			throw new NotFoundException(nameof(Customer),$"with id \"{request.CustomerId}\"" );

		var result = new OnboardCustomerDto();

        //
        result.Id = customer.Id;
		result.Code = customer.Code;
        result.CustomCode = customer.CustomCode;
        result.Name = customer.Name;
		result.RegistrationCertificateNumber = customer.RegistrationCertificateNumber;
		result.IncorporationDate = customer.IncorporationDate.Value;
		result.RegisterAddress1 = customer.RegisterAddress1;
        result.RegisterAddress2 = customer.RegisterAddress2;
        result.TaxIdentificationNumber = customer.TaxIdentificationNumber;
		result.CountryId = customer.CountryId;
		result.Country = customer.Country;
		result.StateId = customer.StateId;
		result.State = customer.State;
        result.City = customer.City;
        result.BusinessNature = customer.BusinessNature;
        result.OfficePhoneCallCode = customer.OfficePhoneCallCode;
		result.OfficePhoneNumber = customer.OfficePhoneNumber;
		result.MobilePhoneCallCode = customer.MobilePhoneCallCode;
		result.MobilePhoneNumber = customer.MobilePhoneNumber;
		result.Email = customer.Email;
		result.Fax = customer.Fax;
		result.Website = customer.Website;
		result.SectorId = customer.SectorId;
		result.SectorName = customer.SectorName;
		result.IndustryId = customer.IndustryId;
		result.IndustryName = customer.IndustryName;
		result.InstitutionTypeId = customer.InstitutionTypeId;
        result.InstitutionTypeName = customer.InstitutionTypeName;
		result.HasChildInstitutionType = customer.HasChildInstitutionType;
		result.ChildInstitutionTypeId = customer.ChildInstitutionTypeId;
        result.ChildInstitutionTypeName = customer.ChildInstitutionTypeName;
        result.InstitutionCode = customer.InstitutionCode;
        result.StaffSize = customer.StaffSize.GetDomainStaffSize();
        result.IsPublic = customer.IsPublic;
        result.Status = customer.Status.GetCustomerStatus();
		result.IsLock = customer.IsLock;
        result.DueDiligenceCompleted = customer.DueDiligenceCompleted;

		result.SettlementBankCode = customer.SettlementBankCode;
		result.SettlementBankName = customer.SettlementBankName;

		result.IsCorrespondentBank = customer.IsCorrespondentBank;
		result.HasSubsidiary = customer.HasSubsidiary;
		result.IsSubsidiary = customer.IsSubsidiary;
		result.ParentName = customer.ParentName;
		result.ParentCode = customer.ParentCode;
		//result.ParentInstitutionCode = customer.Parent != null ? customer.Parent.InstitutionCode : "";

		result.LogoLocation = customer.LogoLocation;

		//Dowload logo
		var Inspector = new ContentInspectorBuilder()
		{
			Definitions = MimeDetective.Definitions.Default.All()
		}.Build();

		if (ValidateUrlWithUri(customer.LogoLocation))
		{
			var logoBytes = await DownloadFile(customer.LogoLocation);
			if (logoBytes != null)
			{
				result.LogoFileData = logoBytes;
				result.LogoSize = logoBytes.Length;

				var mimeResults = Inspector.Inspect(logoBytes);

				if (mimeResults != null && mimeResults.Count() > 0)
				{
					//result.LogoContentType = mimeResults[0].Definition.File.MimeType;
					result.LogoContentType = mimeResults[0].Definition.File.Extensions[0];
				}
			}
		}
			
		//ContactPersons
		foreach (var item in customer.CustomerContactPersons)
		{
			result.ContactPersons.Add(new UpsertContactPersonDto
			{
				FirstName = item.FirstName,
				MiddleName = item.MiddleName,
				LastName = item.LastName,	
				Name = item.Name,
				Email = item.Email,
				MobilePhoneCallCode = item.MobilePhoneCallCode,
				MobilePhoneNumber = item.MobilePhoneNumber,
				Designation = item.Designation,
				Default = item.Default
			});
		}

		//ContactChannels
		foreach (var item in customer.CustomerContactChannels)
		{
			result.ContactChannels.Add(new UpsertContactChannelDto
			{
				Type = item.ChannelType == Domain.Enums.ChannelType.Email ? ChannelType.Email : ChannelType.Phone,
				Email = item.Email,
				MobilePhoneCallCode = item.MobilePhoneCallCode,
				MobilePhoneNumber = item.MobilePhoneNumber
			});
		}

        //Signatories
        foreach (var item in customer.CustomerSignatories)
        {
            result.Signatories.Add(new UpsertSignatoryDto
            {
				FirstName = item.FirstName,
				MiddleName = item.MiddleName,
				LastName = item.LastName,
				Name = item.Name,
                Email = item.Email,
                MobilePhoneCallCode = item.MobileNumberCallCode,
                MobilePhoneNumber = item.MobileNumber,
				Designation = item.Designation
            });
        }

        //Accounts
        foreach (var item in customer.CustomerAccounts)
		{
			result.BankAccounts.Add(new UpsertBankAccountDto
			{
				AccountType = item.AccountType.GetDomainAccountType(),
				BankName = item.BankName,
				BankCode = item.BankCode,
				Country = item.Country,
				BankAddress = item.BankAddress,
				AccountName = item.AccountName,
				AccountNumber = item.AccountNumber,
				IsLocalAccount = item.IsLocalAccount,
				Validated = item.Validated,
			});
		}

		//Documents
		foreach (var item in customer.CustomerDocuments)
		{
			var document = new UpsertDocumentDto
			{
				DocumentTypeId = item.DocumentTypeId,
				DocumentTypeName = item.Title,
				Title = item.Title,
				HasIssueDate = item.HasIssueDate,
				IssueDate = item.IssueDate,
				HasExpiryDate = item.HasExpiryDate,
				ExpiryDate = item.ExpiryDate,
				DocumentId = item.Id,
				LocationUrl = item.Location,
			};

			if (ValidateUrlWithUri(item.Location))
			{
				//Dowload document
				var fileBytes = await DownloadFile(item.Location);
				if (fileBytes != null)
				{
					document.FileData = fileBytes;
					document.Size = fileBytes.Length;

					var mimeResults = Inspector.Inspect(fileBytes);
					if (mimeResults != null && mimeResults.Count() > 0)
					{
						//document.ContentType = mimeResults[0].Definition.File.MimeType;
						document.ContentType = mimeResults[0].Definition.File.Extensions[0];
					}
				}
			}			

			result.Documents.Add(document);
		}

        //Fields
        foreach (var item in customer.CustomertCustomFields)
        {
            result.CustomFields.Add(new UpsertCustomFieldDto
            {
                CustomFieldId = item.CustomFieldId,
                CustomField = item.CustomField,
                IsCompulsory = item.IsCompulsory,
                Response = item.Response
            });
        }

        response.Result = result;

		return response;
	}

    private bool ValidateQuery(GetCustomerDetailByIdQuery query)
    {
        var hashMode = _configuration["HashSettings:Mode"].ToString();
        var hashKey = _configuration["HashSettings:Key"].ToString();

        var hashInput = $"{query.CustomerId}{hashKey}";

        if (hashMode == "Sha256")
        {
            return HashUtility.ValidateSha256Hash(query.Hash, hashInput);
        }
        else if (hashMode == "MD5")
        {
            return HashUtility.ValidateMD5Hash(query.Hash, hashInput);
        }
        else
        {
            throw new NotImplementedException("Hash mode not implemented.");
        }
    }

    private OnboardingStatus GetCustomerStatus(Domain.Enums.OnboardingStatus status)
	{
		if (status == Domain.Enums.OnboardingStatus.NotStarted)
			return OnboardingStatus.NotStarted;
		else if (status == Domain.Enums.OnboardingStatus.Submitted)
			return OnboardingStatus.Submitted;
		else if (status == Domain.Enums.OnboardingStatus.Processing)
			return OnboardingStatus.Processing;
		else if (status == Domain.Enums.OnboardingStatus.Queried)
			return OnboardingStatus.Queried;
		else if (status == Domain.Enums.OnboardingStatus.Completed)
			return OnboardingStatus.Completed;
		else
			throw new NotImplementedException();
	}

	private async Task<byte[]> DownloadFile(string absoluteUrl)
	{
		using (var client = new HttpClient())
		{
			return await client.GetByteArrayAsync(absoluteUrl);
		}
	}

	private bool ValidateUrlWithUri(string url)
	{
		return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
			&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
	}
}