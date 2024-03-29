using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Models;
using Fintrak.CustomerPortal.Application.Queries.Commands;
using Fintrak.CustomerPortal.Application.Utilities;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeDetective;
using System.Linq;
using System.Text;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

public record class GetCustomersQuery(string? SearchText, DateTime? StartDate, DateTime? EndDate, OnboardingStatus? Status, int? PageIndex = 1, int? PageSize = 10) : IRequest<BaseResponse<PaginatedList<OnboardCustomerDto>>>;

public class GetCustomersQueryHandler : IRequestHandler<GetCustomersQuery, BaseResponse<PaginatedList<OnboardCustomerDto>>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public GetCustomersQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper, IConfiguration configuration)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
        _configuration = configuration;
    }

	public async Task<BaseResponse<PaginatedList<OnboardCustomerDto>>> Handle(GetCustomersQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<PaginatedList<OnboardCustomerDto>>();

        var query = _context.Customers
			.Include(c=> c.CustomerContactPersons)
			.Include(c => c.CustomerContactChannels)
			.Include(c => c.CustomerAccounts)
			.Include(c => c.CustomerSignatories)
			.Include(c => c.CustomertCustomFields)
            .Include(c=> c.Parent)
			.Include(c => c.CustomerDocuments).OrderByDescending(c => c.Created).AsQueryable();

        if (!string.IsNullOrEmpty(request.SearchText))
        {
            query = query.Where(c=> c.Name.Contains(request.SearchText) || c.RegistrationCertificateNumber.Contains(request.SearchText));
        }

        
        if(request.Status.HasValue) 
        {
            var statusSearch = request.Status.Value.GetCustomerDomainStatus();
			query = query.Where(c => c.Status == statusSearch);
        }

        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            query = query.Where(c => c.Created >= request.StartDate.Value && c.Created <= request.EndDate.Value);
        }

        var customers = await query.Skip((request.PageIndex.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value).AsNoTracking().ToListAsync();

        var customerCount = await query.CountAsync();

        var list = new List<OnboardCustomerDto>();

        foreach (var customer in customers)
		{
            var listItem = new OnboardCustomerDto();

            //
            listItem.Id = customer.Id;
            listItem.Code = customer.Code;
            listItem.CustomCode = customer.CustomCode;
            listItem.Name = customer.Name;
            listItem.RegistrationCertificateNumber = customer.RegistrationCertificateNumber;
            listItem.IncorporationDate = customer.IncorporationDate.Value;
            listItem.RegisterAddress1 = customer.RegisterAddress1;
            listItem.RegisterAddress2 = customer.RegisterAddress2;
            listItem.TaxIdentificationNumber = customer.TaxIdentificationNumber;
			listItem.CountryId = customer.CountryId;
			listItem.Country = customer.Country;
			listItem.StateId = customer.StateId;
			listItem.State = customer.State;
			listItem.City = customer.City;
            listItem.BusinessNature = customer.BusinessNature;
            listItem.OfficePhoneCallCode = customer.OfficePhoneCallCode;
            listItem.OfficePhoneNumber = customer.OfficePhoneNumber;
            listItem.MobilePhoneCallCode = customer.MobilePhoneCallCode;
            listItem.MobilePhoneNumber = customer.MobilePhoneNumber;
            listItem.Email = customer.Email;
            listItem.Fax = customer.Fax;
            listItem.Website = customer.Website;
            listItem.SectorId = customer.SectorId;
            listItem.SectorName = customer.SectorName;
            listItem.IndustryId = customer.IndustryId;
            listItem.IndustryName = customer.IndustryName;
            listItem.InstitutionTypeId = customer.InstitutionTypeId;
            listItem.InstitutionTypeName = customer.InstitutionTypeName;
			listItem.HasChildInstitutionType = customer.HasChildInstitutionType;
			listItem.ChildInstitutionTypeId = customer.ChildInstitutionTypeId;
            listItem.ChildInstitutionTypeName = customer.ChildInstitutionTypeName;
            listItem.InstitutionCode = customer.InstitutionCode;
            listItem.StaffSize = customer.StaffSize.GetDomainStaffSize();
            listItem.IsPublic = customer.IsPublic;
            listItem.Status = customer.Status.GetCustomerStatus();
            listItem.IsLock = customer.IsLock;
            listItem.LogoLocation = customer.LogoLocation;

			listItem.SettlementBankCode = customer.SettlementBankCode;
			listItem.SettlementBankName = customer.SettlementBankName;

			listItem.IsCorrespondentBank = customer.IsCorrespondentBank;
			listItem.HasSubsidiary = customer.HasSubsidiary;
			listItem.IsSubsidiary = customer.IsSubsidiary;
			//listItem.ParentId = customer.ParentId;
			listItem.ParentName = customer.ParentName;
			listItem.ParentCode = customer.ParentCode;
			//result.ParentInstitutionCode = customer.Parent != null ? customer.Parent.InstitutionCode : "";

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
					listItem.LogoFileData = logoBytes;
					listItem.LogoSize = logoBytes.Length;
                    listItem.LogoSelectedFileName = $"pc{customer.Id}_logo".ToLower();

					var mimeResults = Inspector.Inspect(logoBytes);

					if (mimeResults != null && mimeResults.Count() > 0)
					{
						//listItem.LogoContentType = mimeResults[0].Definition.File.MimeType;
						listItem.LogoContentType = mimeResults[0].Definition.File.Extensions[0];
					}
				}
			}

			//ContactPersons
			foreach (var item in customer.CustomerContactPersons)
            {
                listItem.ContactPersons.Add(new UpsertContactPersonDto
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
                listItem.ContactChannels.Add(new UpsertContactChannelDto
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
                listItem.Signatories.Add(new UpsertSignatoryDto
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
                listItem.BankAccounts.Add(new UpsertBankAccountDto
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
					
				listItem.Documents.Add(document);
            }

            //Fields
            foreach (var item in customer.CustomertCustomFields)
            {
                listItem.CustomFields.Add(new UpsertCustomFieldDto
                {
                    CustomFieldId = item.CustomFieldId,
                    CustomField = item.CustomField,
                    IsCompulsory = item.IsCompulsory,
                    Response = item.Response
                });
            }

            list.Add(listItem);
        }

		response.Result = new PaginatedList<OnboardCustomerDto>(list,customerCount,request.PageIndex.Value, request.PageSize.Value);

		return response;
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