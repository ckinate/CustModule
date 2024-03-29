using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Security;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Fintrak.CustomerPortal.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;

[Authorize]
public record class GetCustomerDetailQuery : IRequest<BaseResponse<OnboardCustomerDto>>;

public class GetCustomerDetailQueryHandler : IRequestHandler<GetCustomerDetailQuery, BaseResponse<OnboardCustomerDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetCustomerDetailQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<BaseResponse<OnboardCustomerDto>> Handle(GetCustomerDetailQuery request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<OnboardCustomerDto>();

		var loginId = _currentUserService.UserId;
		var customer = await _context.Customers
			.Include(c=> c.CustomerContactPersons)
			.Include(c => c.CustomerContactChannels)
			.Include(c => c.CustomerAccounts)
			.Include(c => c.CustomerSignatories)
			.Include(c => c.CustomerDocuments)
			.Include(c => c.CustomertCustomFields)
			.Include(c => c.Parent)
			.FirstOrDefaultAsync(c => c.LoginId == loginId);

		//if (customer == null)
		//	throw new NotFoundException(nameof(Customer),$"with user id \"{loginId}\"" );
		if (customer != null)
		{
			var result = new OnboardCustomerDto();

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
			result.LogoLocation = customer.LogoLocation;

			result.SettlementBankCode = customer.SettlementBankCode;
			result.SettlementBankName = customer.SettlementBankName;

			result.IsCorrespondentBank = customer.IsCorrespondentBank;
			result.HasSubsidiary = customer.HasSubsidiary;
			result.IsSubsidiary = customer.IsSubsidiary;
			result.ParentId = customer.ParentId;
			result.ParentName = customer.ParentName;
			result.ParentCode = customer.ParentCode;
			//result.ParentInstitutionCode = customer.Parent != null ? customer.Parent.InstitutionCode : "";

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
				result.Documents.Add(new UpsertDocumentDto
				{
					DocumentTypeId = item.DocumentTypeId,
					DocumentTypeName = item.Title,
					Title = item.Title,
                    HasIssueDate = item.HasIssueDate,
					IssueDate = item.IssueDate,
					HasExpiryDate = item.HasExpiryDate,
					ExpiryDate = item.ExpiryDate,
					LocationUrl = item.Location,
					DocumentId = item.Id,
				});
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

            var query = await _context.Queries.Where(c=> c.ResourceType == Domain.Enums.ResourceType.Customer && c.ResourceReference == customer.Id.ToString() && c.RequireDataModification).FirstOrDefaultAsync();
            if (query != null)
            {
				result.CanUpdate = true;
			}

            response.Result = result;
		}

		return response;
	}
}