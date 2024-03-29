using Fintrak.CustomerPortal.Application.Common.Exceptions;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Fintrak.CustomerPortal.Domain.Entities;
using Fintrak.CustomerPortal.Domain.Events.Onboarding;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Fintrak.CustomerPortal.Application.Onboarding.Commands;

public record SaveOnboardCustomerCommand : IRequest<BaseResponse<int>>
{
    public int CurrentStep { get; set; }
    public OnboardCustomerDto Item { get; set; }
	public string NotificationEmail { get; set; }
}

public class SaveOnboardCustomerCommandHandler : IRequestHandler<SaveOnboardCustomerCommand, BaseResponse<int>>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
    private readonly IIdentityService _identityService;
    private readonly IFileStore _fileStore;

	public SaveOnboardCustomerCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService, IFileStore fileStore)
	{
		_context = context;
		_currentUserService = currentUserService;
		_identityService = identityService;
        _fileStore = fileStore;
	}

	public async Task<BaseResponse<int>> Handle(SaveOnboardCustomerCommand request, CancellationToken cancellationToken)
	{
		var response = new BaseResponse<int>();

		Customer entity = null;

        var user = await _identityService.GetUserAsync(_currentUserService.UserId);
        if (user == null)
        {
            response.Success = false;
            response.Message = "Unable to load customer's profile.";
            return response;
        }

        //process step 1
        var step1Response = await ProcessStep1(request, user, cancellationToken);
        if(step1Response.Success && step1Response.Result != null)
        {
            entity = step1Response.Result;
        }
        else
        {
            response.Success = step1Response.Success;
            response.Message = step1Response.Message;
            return response;
        }

        //Process step 2
        var step2Response = await ProcessStep2(request, user, entity, cancellationToken);
        if (!step2Response.Success)
        {
            return step2Response;
        }

        //Process step 3
        var step3Response = await ProcessStep3(request, user, entity, cancellationToken);
        if (!step3Response.Success)
        {
            return step3Response;
        }

        //Process step 4
        var step4Response = await ProcessStep4(request, user, entity, cancellationToken);
        if (!step4Response.Success)
        {
            return step4Response;
        }

        //Process step 5
        var step5Response = await ProcessStep5(request, user, entity, cancellationToken);
        if (!step5Response.Success)
        {
            return step5Response;
        }

        //Process step 6
        var step6Response = await ProcessStep6(request, user, entity, cancellationToken);
        if (!step6Response.Success)
        {
            return step6Response;
        }

        //Process step 7
        var step7Response = await ProcessStep7(request, user, entity, cancellationToken);
        if (!step7Response.Success)
        {
            return step7Response;
        }

		//Process step 8
		var step8Response = await ProcessStep8(request, user, entity, cancellationToken);
		if (!step8Response.Success)
		{
			return step8Response;
		}

		response.Result = entity.Id;

		return response;
	}

	public async Task<BaseResponse<Customer>> ProcessStep1(SaveOnboardCustomerCommand request, UserDto user, CancellationToken cancellationToken)
	{
        var response = new BaseResponse<Customer>();

        var entity = await _context.Customers.FirstOrDefaultAsync(c => c.LoginId == _currentUserService.UserId);

        if (request.CurrentStep == 1)
		{
			if (request.Item.IsSubsidiary)
			{
				if (string.IsNullOrEmpty(request.Item.ParentCode) || string.IsNullOrEmpty(request.Item.ParentName))
				{
					response.Success = false;
					response.Message = $"Group head is required.";

					return response;
				}
			}

			var entityWithEmailExist = await _context.Customers.FirstOrDefaultAsync(c => c.Email == request.Item.Email);
            var entityWithRCExist = await _context.Customers.FirstOrDefaultAsync(c => c.RegistrationCertificateNumber == request.Item.RegistrationCertificateNumber);
            var entityWithTinExist = await _context.Customers.FirstOrDefaultAsync(c => c.TaxIdentificationNumber == request.Item.TaxIdentificationNumber);
            var entityWithPhoneExist = await _context.Customers.FirstOrDefaultAsync(c => c.OfficePhoneCallCode == request.Item.OfficePhoneCallCode && c.OfficePhoneNumber == request.Item.OfficePhoneNumber);
            var entityWithMobileExist = await _context.Customers.FirstOrDefaultAsync(c => c.MobilePhoneCallCode == request.Item.MobilePhoneCallCode && c.MobilePhoneNumber == request.Item.MobilePhoneNumber);

			if (request.Item.IsSubsidiary)
			{
				if (string.IsNullOrEmpty(request.Item.ParentCode) || string.IsNullOrEmpty(request.Item.ParentName))
				{
					response.Success = false;
					response.Message = $"Group head is required.";

					return response;
				}
			}

			if (entity == null)
            {
                if (entityWithEmailExist != null)
                {
                    response.Success = false;
                    response.Message = $"Customer with email '{request.Item.Email}' already exist.";

                    return response;
                }

                if (entityWithRCExist != null)
                {
                    response.Success = false;
                    response.Message = $"Customer with RC '{request.Item.RegistrationCertificateNumber}' already exist.";

                    return response;
                }

                if (entityWithTinExist != null)
                {
                    response.Success = false;
                    response.Message = $"Customer with Tin '{request.Item.TaxIdentificationNumber}' already exist.";

                    return response;
                }

                if (entityWithPhoneExist != null)
                {
                    response.Success = false;
                    response.Message = $"Customer with Office Phone '{request.Item.OfficePhoneCallCode}-{request.Item.OfficePhoneNumber}' already exist.";

                    return response;
                }

                if (entityWithMobileExist != null)
                {
                    response.Success = false;
                    response.Message = $"Customer with Mobile Phone '{request.Item.MobilePhoneCallCode}-{request.Item.MobilePhoneNumber}' already exist.";

                    return response;
                }

                entity = new Customer
                {
                    Name = request.Item.Name,
                    RegistrationCertificateNumber = request.Item.RegistrationCertificateNumber,
                    IncorporationDate = request.Item.IncorporationDate,
                    RegisterAddress1 = request.Item.RegisterAddress1,
                    RegisterAddress2 = request.Item.RegisterAddress2,
                    TaxIdentificationNumber = request.Item.TaxIdentificationNumber,
                    CountryId = request.Item.CountryId,
                    Country = request.Item.Country,
                    StateId = request.Item.StateId,
                    State = request.Item.State,
                    City = request.Item.City,
                    BusinessNature = request.Item.BusinessNature,
                    OfficePhoneCallCode = request.Item.OfficePhoneCallCode,
                    OfficePhoneNumber = request.Item.OfficePhoneNumber,
                    MobilePhoneCallCode = request.Item.MobilePhoneCallCode,
                    MobilePhoneNumber = request.Item.MobilePhoneNumber,
                    Email = request.Item.Email,
                    Fax = request.Item.Fax,
                    Website = request.Item.Website,
                    SectorId = request.Item.SectorId,
                    SectorName = request.Item.SectorName,
                    IndustryId = request.Item.IndustryId,
                    IndustryName = request.Item.IndustryName,
                    InstitutionTypeId = request.Item.InstitutionTypeId,
                    InstitutionTypeName = request.Item.InstitutionTypeName,
					HasChildInstitutionType = request.Item.HasChildInstitutionType,
					ChildInstitutionTypeId = request.Item.ChildInstitutionTypeId,
                    ChildInstitutionTypeName = request.Item.ChildInstitutionTypeName,
                    InstitutionCode = request.Item.InstitutionCode,
                    StaffSize = request.Item.StaffSize.GetDomainStaffSize(),
                    RegistrationDate = DateTime.Now,
                    IsPublic = request.Item.IsPublic,
                    LoginId = _currentUserService.UserId,
                    Status = Domain.Enums.OnboardingStatus.NotStarted,
                    RequireSync = true,
                    SettlementBankCode = request.Item.SettlementBankCode,
                    SettlementBankName = request.Item.SettlementBankName,

                    IsCorrespondentBank = request.Item.IsCorrespondentBank,
                    HasSubsidiary = request.Item.HasSubsidiary, 
                    IsSubsidiary =  request.Item.IsSubsidiary,
                    ParentCode = request.Item.ParentCode,
					ParentName = request.Item.ParentName
				};

                //logo
                entity.AddDomainEvent(new CustomerOnboardCompletedEvent(entity, request.NotificationEmail, user.AdminName, user.Email, true));
                _context.Customers.Add(entity);
            }
            else
            {
                if (entityWithEmailExist != null && entityWithEmailExist.Id != entity.Id)
                {
                    response.Success = false;
                    response.Message = $"Customer with email '{request.Item.Email}' already exist.";

                    return response;
                }

                if (entityWithRCExist != null && entityWithRCExist.Id != entity.Id)
                {
                    response.Success = false;
                    response.Message = $"Customer with RC '{request.Item.RegistrationCertificateNumber}' already exist.";

                    return response;
                }

                if (entityWithTinExist != null && entityWithTinExist.Id != entity.Id)
                {
                    response.Success = false;
                    response.Message = $"Customer with Tin '{request.Item.TaxIdentificationNumber}' already exist.";

                    return response;
                }

                if (entityWithPhoneExist != null && entityWithPhoneExist.Id != entity.Id)
                {
                    response.Success = false;
                    response.Message = $"Customer with Office Phone '{request.Item.OfficePhoneCallCode}-{request.Item.OfficePhoneNumber}' already exist.";

                    return response;
                }

                if (entityWithMobileExist != null && entityWithMobileExist.Id != entity.Id)
                {
                    response.Success = false;
                    response.Message = $"Customer with Mobile Phone '{request.Item.MobilePhoneCallCode}-{request.Item.MobilePhoneNumber}' already exist.";

                    return response;
                }

                entity.Name = request.Item.Name;
                entity.RegistrationCertificateNumber = request.Item.RegistrationCertificateNumber;
                entity.IncorporationDate = request.Item.IncorporationDate;
                entity.RegisterAddress1 = request.Item.RegisterAddress1;
                entity.RegisterAddress2 = request.Item.RegisterAddress2;
                entity.TaxIdentificationNumber = request.Item.TaxIdentificationNumber;
                entity.CountryId = request.Item.CountryId;
                entity.Country = request.Item.Country;
                entity.StateId = request.Item.StateId;
                entity.State = request.Item.State;
                entity.City = request.Item.City;
                entity.BusinessNature = request.Item.BusinessNature;
                entity.OfficePhoneCallCode = request.Item.OfficePhoneCallCode;
                entity.OfficePhoneNumber = request.Item.OfficePhoneNumber;
                entity.MobilePhoneCallCode = request.Item.MobilePhoneCallCode;
                entity.MobilePhoneNumber = request.Item.MobilePhoneNumber;
                entity.Email = request.Item.Email;
                entity.Fax = request.Item.Fax;
                entity.Website = request.Item.Website;
                entity.SectorId = request.Item.SectorId;
                entity.SectorName = request.Item.SectorName;
                entity.IndustryId = request.Item.IndustryId;
                entity.IndustryName = request.Item.IndustryName;
                entity.InstitutionTypeId = request.Item.InstitutionTypeId;
                entity.InstitutionTypeName = request.Item.InstitutionTypeName;
				entity.HasChildInstitutionType = request.Item.HasChildInstitutionType;
				entity.ChildInstitutionTypeId = request.Item.ChildInstitutionTypeId;
                entity.ChildInstitutionTypeName = request.Item.ChildInstitutionTypeName;
                entity.InstitutionCode = request.Item.InstitutionCode;
                entity.StaffSize = request.Item.StaffSize.GetDomainStaffSize();
                entity.IsPublic = request.Item.IsPublic;
                entity.RequireSync = true;
				entity.SettlementBankCode = request.Item.SettlementBankCode;
				entity.SettlementBankName = request.Item.SettlementBankName;

				entity.IsCorrespondentBank = request.Item.IsCorrespondentBank;
				entity.HasSubsidiary = request.Item.HasSubsidiary;
				entity.IsSubsidiary = request.Item.IsSubsidiary;
				entity.ParentCode = request.Item.ParentCode;
				entity.ParentName = request.Item.ParentName;

				//Logo

				entity.AddDomainEvent(new CustomerOnboardCompletedEvent(entity, request.NotificationEmail, user.AdminName, user.Email));

                var queries = await _context.Queries.Where(c => c.CustomerId == entity.Id && c.RequireDataModification).ToListAsync();
                foreach (var query in queries)
                {
                    query.RequireDataModification = false;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

        }

		var logoLocationUrl = string.Empty;
		
        
        if (request.Item.LogoFileData != null)
		{
			logoLocationUrl = await _fileStore.UploadFile(request.Item.LogoFileData, $"logo_customer_{entity.Id}", request.Item.LogoContentType, "customers");
			if (string.IsNullOrEmpty(logoLocationUrl))
			{
				throw new FileUploadException($"logo_customer_{entity.Id}", $"Customer: {entity.Name}");
			}

            if(ValidateUrlWithUri(logoLocationUrl))
			    entity.LogoLocation = logoLocationUrl;

			await _context.SaveChangesAsync(cancellationToken);
		}

		response.Result = entity;

        return response;
    }

    public async Task<BaseResponse<int>> ProcessStep2(SaveOnboardCustomerCommand request, UserDto user, Customer entity, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<int>();

        if (request.CurrentStep == 2)
        {
            if (request.Item.ContactPersons.Count > 0)
            {
                var contactNames = request.Item.ContactPersons.Select(c => c.Name).Distinct().ToList();
                if (contactNames.Count != request.Item.ContactPersons.Count)
                {
                    response.Success = false;
                    response.Message = $"All contact names must be unique.";

                    return response;
                }

                var contactEmails = request.Item.ContactPersons.Select(c => c.Email).Distinct().ToList();
                if (contactEmails.Count != request.Item.ContactPersons.Count)
                {
                    response.Success = false;
                    response.Message = $"All contact emails must be unique.";

                    return response;
                }

                var contactPhones = request.Item.ContactPersons.Select(c => $"{c.MobilePhoneCallCode} - {c.MobilePhoneNumber}").Distinct().ToList();
                if (contactPhones.Count != request.Item.ContactPersons.Count)
                {
                    response.Success = false;
                    response.Message = $"All contact phones must be unique.";

                    return response;
                }
            }

            var existingContactPersons = await _context.CustomerContactPersons.Where(c => c.CustomerId == entity.Id).ToListAsync();
            if (existingContactPersons.Any())
            {
                _context.CustomerContactPersons.RemoveRange(existingContactPersons);
                await _context.SaveChangesAsync(cancellationToken);
            }

            foreach (var contactPerson in request.Item.ContactPersons)
            {
                var newContactPerson = new CustomerContactPerson
                {
                    CustomerId = entity.Id,
                    FirstName = contactPerson.FirstName,
                    MiddleName = contactPerson.MiddleName,
                    LastName = contactPerson.LastName,
                    Name = contactPerson.Name,
                    Email = contactPerson.Email,
                    MobilePhoneCallCode = contactPerson.MobilePhoneCallCode,
                    MobilePhoneNumber = contactPerson.MobilePhoneNumber,
                    Designation = contactPerson.Designation,
                    Default = contactPerson.Default,
                };

                _context.CustomerContactPersons.Add(newContactPerson);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        return response;
    }

    public async Task<BaseResponse<int>> ProcessStep3(SaveOnboardCustomerCommand request, UserDto user, Customer entity, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<int>();

        if (request.CurrentStep == 3)
        {
            if (request.Item.ContactChannels.Count > 0)
            {
                var channelEmails = request.Item.ContactChannels.Where(c => c.Type == ChannelType.Email).Select(c => c.Email).Distinct().ToList();
                if (channelEmails.Count != request.Item.ContactChannels.Where(c => c.Type == ChannelType.Email).ToList().Count)
                {
                    response.Success = false;
                    response.Message = $"All channel emails must be unique.";

                    return response;
                }

                var channelPhones = request.Item.ContactChannels.Where(c => c.Type == ChannelType.Phone).Select(c => $"{c.MobilePhoneCallCode} - {c.MobilePhoneNumber}").Distinct().ToList();
                if (channelPhones.Count != request.Item.ContactChannels.Where(c => c.Type == ChannelType.Phone).ToList().Count)
                {
                    response.Success = false;
                    response.Message = $"All channel phones must be unique.";

                    return response;
                }
            }

            var existingContactChannels = await _context.CustomerContactChannels.Where(c => c.CustomerId == entity.Id).ToListAsync();
            if (existingContactChannels.Any())
            {
                _context.CustomerContactChannels.RemoveRange(existingContactChannels);
                await _context.SaveChangesAsync(cancellationToken);
            }

            foreach (var contactChannel in request.Item.ContactChannels)
            {
                var newContactChannel = new CustomerContactChannel
                {
                    CustomerId = entity.Id,
                    ChannelType = contactChannel.Type == ChannelType.Email ? Domain.Enums.ChannelType.Email : Domain.Enums.ChannelType.Phone,
                    Email = contactChannel.Email,
                    MobilePhoneCallCode = contactChannel.MobilePhoneCallCode,
                    MobilePhoneNumber = contactChannel.MobilePhoneNumber,
                };

                _context.CustomerContactChannels.Add(newContactChannel);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        return response;
    }

    public async Task<BaseResponse<int>> ProcessStep4(SaveOnboardCustomerCommand request, UserDto user, Customer entity, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<int>();

        if (request.CurrentStep == 4)
        {
            var inputCount = request.Item.BankAccounts.Where(c => c.AccountType == AccountType.Fee).Count();
            var accountNumbers = request.Item.BankAccounts.Where(c => c.AccountType == AccountType.Fee).Select(c => c.AccountNumber).Distinct().ToList();
            if (accountNumbers.Count != inputCount)
            {
                response.Success = false;
                response.Message = $"All fee account numbers must be unique.";

                return response;
            }

            accountNumbers = request.Item.BankAccounts.Where(c => c.AccountType == AccountType.Fee).Select(c => c.AccountNumber).Distinct().ToList();
            if (await _context.CustomerAccounts.AnyAsync(c => c.AccountType == Domain.Enums.AccountType.Fee && c.CustomerId != entity.Id && accountNumbers.Contains(c.AccountNumber)))
            {
                response.Success = false;
                response.Message = $"One or more of the fee account numbers already exist.";

                return response;
            }

            var existingCustomerAccounts = await _context.CustomerAccounts.Where(c => c.CustomerId == entity.Id && c.AccountType == Domain.Enums.AccountType.Fee).ToListAsync();
            if (existingCustomerAccounts.Any())
            {
                _context.CustomerAccounts.RemoveRange(existingCustomerAccounts);

                await _context.SaveChangesAsync(cancellationToken);
            }

            foreach (var bankAccount in request.Item.BankAccounts.Where(c=> c.AccountType == AccountType.Fee).ToList())
            {
                var newCustomerAccount = new CustomerAccount
                {
                    CustomerId = entity.Id,
                    AccountType = bankAccount.AccountType.GetDomainAccountType(),
                    BankName = bankAccount.BankName,
                    BankCode = bankAccount.BankCode,
                    BankAddress = bankAccount.BankAddress,
                    AccountName = bankAccount.AccountName,
                    AccountNumber = bankAccount.AccountNumber,
                    Country = bankAccount.Country,
                    IsLocalAccount = bankAccount.IsLocalAccount,
                    Validated = bankAccount.Validated,
                };

                _context.CustomerAccounts.Add(newCustomerAccount);
            }

            await _context.SaveChangesAsync(cancellationToken);

        }

        return response;
    }

    public async Task<BaseResponse<int>> ProcessStep5(SaveOnboardCustomerCommand request, UserDto user, Customer entity, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<int>();

        if (request.CurrentStep == 5)
        {
            var inputCount = request.Item.BankAccounts.Where(c => c.AccountType == AccountType.Commission).Count();
            var accountNumbers = request.Item.BankAccounts.Where(c => c.AccountType == AccountType.Commission).Select(c => c.AccountNumber).Distinct().ToList();
            if (accountNumbers.Count != inputCount)
            {
                response.Success = false;
                response.Message = $"All commission account numbers must be unique.";

                return response;
            }

            accountNumbers = request.Item.BankAccounts.Where(c => c.AccountType == AccountType.Commission).Select(c => c.AccountNumber).Distinct().ToList();
            if (await _context.CustomerAccounts.AnyAsync(c => c.AccountType == Domain.Enums.AccountType.Commission && c.CustomerId != entity.Id && accountNumbers.Contains(c.AccountNumber)))
            {
                response.Success = false;
                response.Message = $"One or more of the commission account numbers already exist.";

                return response;
            }

            var existingCustomerAccounts = await _context.CustomerAccounts.Where(c => c.CustomerId == entity.Id && c.AccountType == Domain.Enums.AccountType.Commission).ToListAsync();
            if (existingCustomerAccounts.Any())
            {
                _context.CustomerAccounts.RemoveRange(existingCustomerAccounts);

                await _context.SaveChangesAsync(cancellationToken);
            }

            foreach (var bankAccount in request.Item.BankAccounts.Where(c => c.AccountType == AccountType.Commission).ToList())
            {
                var newCustomerAccount = new CustomerAccount
                {
                    CustomerId = entity.Id,
                    AccountType = bankAccount.AccountType.GetDomainAccountType(),
                    BankName = bankAccount.BankName,
                    BankCode = bankAccount.BankCode,
                    BankAddress = bankAccount.BankAddress,
                    AccountName = bankAccount.AccountName,
                    AccountNumber = bankAccount.AccountNumber,
                    Country = bankAccount.Country,
                    IsLocalAccount = bankAccount.IsLocalAccount,
                    Validated = bankAccount.Validated,
                };

                _context.CustomerAccounts.Add(newCustomerAccount);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        return response;
    }

    public async Task<BaseResponse<int>> ProcessStep6(SaveOnboardCustomerCommand request, UserDto user, Customer entity, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<int>();

        if (request.CurrentStep == 6)
        {
            var existingSignatories = await _context.CustomerSignatories.Where(c => c.CustomerId == entity.Id).ToListAsync();
            if (existingSignatories.Any())
            {
                _context.CustomerSignatories.RemoveRange(existingSignatories);
                await _context.SaveChangesAsync(cancellationToken);
            }

            foreach (var signatory in request.Item.Signatories)
            {
                var newSignatory = new CustomerSignatory
                {
                    CustomerId = entity.Id,
					FirstName = signatory.FirstName,
					MiddleName = signatory.MiddleName,
					LastName = signatory.LastName,
					Name = signatory.Name,
                    Email = signatory.Email,
                    MobileNumber = signatory.MobilePhoneNumber,
                    MobileNumberCallCode = signatory.MobilePhoneCallCode,
                    Designation = signatory.Designation,
                };

                _context.CustomerSignatories.Add(newSignatory);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        return response;
    }
 
    public async Task<BaseResponse<int>> ProcessStep7(SaveOnboardCustomerCommand request, UserDto user, Customer entity, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<int>();

        if (request.CurrentStep == 7)
        {
            if (request.Item.Documents.Count > 0)
            {
                var documentTitles = request.Item.Documents.Select(c => c.Title).Distinct().ToList();
                if (documentTitles.Count != request.Item.Documents.Count)
                {
                    response.Success = false;
                    response.Message = $"All document title must be unique.";

                    return response;
                }
            }

            var existingDocuments = await _context.CustomerDocuments.Where(c => c.CustomerId == entity.Id).ToListAsync();
            var existingDocumentIds = request.Item.Documents.Where(c => c.DocumentId.HasValue).Select(c => c.DocumentId).Distinct().ToList();
            var documentsToDelete = existingDocuments.Where(c => !existingDocumentIds.Contains(c.Id)).ToList();

            if (documentsToDelete.Any())
            {
                _context.CustomerDocuments.RemoveRange(documentsToDelete);
                await _context.SaveChangesAsync(cancellationToken);
            }

            foreach (var document in request.Item.Documents)
            {
                //process file data

                CustomerDocument existingDocument = null;
                if (document.DocumentId.HasValue)
                    existingDocument = existingDocuments.FirstOrDefault(c => c.Id == document.DocumentId.Value);

                var locationUrl = string.Empty;
                if (document.FileData != null)
                {
                    locationUrl = await _fileStore.UploadFile(document.FileData, $"{document.Title.ToLower()}_customer_{entity.Id}", document.ContentType, "customers");

                    if (string.IsNullOrEmpty(locationUrl))
                    {
                        throw new FileUploadException(document.Title, $"Customer: {entity.Name}");
                    }
				}

                if (existingDocument != null)
                {
                    existingDocument.Title = document.Title;

                    if (!string.IsNullOrEmpty(locationUrl) && ValidateUrlWithUri(locationUrl))
                    {
                        existingDocument.Location = locationUrl;
                    }
                }
                else
                {
                    var newDocument = new CustomerDocument
                    {
                        DocumentTypeId = document.DocumentTypeId,
                        CustomerId = entity.Id,
                        Title = document.Title,
						HasIssueDate = document.HasIssueDate,
						IssueDate = document.IssueDate,
                        HasExpiryDate = document.HasExpiryDate,
                        ExpiryDate = document.ExpiryDate,
                        Source = Fintrak.CustomerPortal.Domain.Enums.DocumentSource.Customer
                    };

					if (!string.IsNullOrEmpty(locationUrl) && ValidateUrlWithUri(locationUrl))
					{
						newDocument.Location = locationUrl;
					}

					_context.CustomerDocuments.Add(newDocument);
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

        }

        return response;
    }

    public async Task<BaseResponse<int>> ProcessStep8(SaveOnboardCustomerCommand request, UserDto user, Customer entity, CancellationToken cancellationToken)
    {
        var response = new BaseResponse<int>();

        if (request.CurrentStep == 8)
        {
            if (request.Item.CustomFields.Count > 0)
            {
                var fields = request.Item.CustomFields.Select(c => c.CustomField).Distinct().ToList();
                if (fields.Count != request.Item.CustomFields.Count)
                {
                    response.Success = false;
                    response.Message = $"All custom field title must be unique.";

                    return response;
                }

                if (request.Item.CustomFields.Any(c => c.IsCompulsory && string.IsNullOrEmpty(c.Response)))
                {
                    response.Success = false;
                    response.Message = $"One or more of the compulsory fields are not filled.";

                    return response;
                }
            }
            //process custom fields
            var existingCustomFields = await _context.CustomerCustomFields.Where(c => c.CustomerId == entity.Id).ToListAsync();
            if (existingCustomFields.Any())
            {
                _context.CustomerCustomFields.RemoveRange(existingCustomFields);
                await _context.SaveChangesAsync(cancellationToken);
            }

            foreach (var customField in request.Item.CustomFields)
            {
                var newField = new CustomerCustomField
                {
                    CustomerId = entity.Id,
                    CustomField = customField.CustomField,
                    CustomFieldId = customField.CustomFieldId,
                    IsCompulsory = customField.IsCompulsory,
                    Response = customField.Response,
                };

                _context.CustomerCustomFields.Add(newField);
            }

            await _context.SaveChangesAsync(cancellationToken);

        }

        return response;
    }

	private bool ValidateUrlWithUri(string url)
	{
		return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
			&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
	}
}
