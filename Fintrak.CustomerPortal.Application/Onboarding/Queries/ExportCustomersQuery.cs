using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Shared.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries
{
    public record class ExportCustomersQuery(string? SearchText, DateTime? StartDate, DateTime? EndDate, OnboardingStatus? Status, int? PageIndex = 1, int? PageSize = 10) : IRequest<ExportDto>;

    public class ExportCustomersQueryHandler : IRequestHandler<ExportCustomersQuery, ExportDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICsvFileBuilder _fileBuilder;

        public ExportCustomersQueryHandler(IApplicationDbContext context, ICsvFileBuilder fileBuilder)
        {
            _context = context;
            _fileBuilder = fileBuilder;
        }

        public async Task<ExportDto> Handle(ExportCustomersQuery request, CancellationToken cancellationToken)
        {
            var result = new ExportDto();

            var query = _context.Customers
            .Include(c => c.CustomerContactPersons)
            .Include(c => c.CustomerContactChannels)
            .Include(c => c.CustomerAccounts)
            .Include(c => c.CustomerSignatories)
            .Include(c => c.CustomerDocuments)
            .Include(c => c.Parent)
            .OrderByDescending(c => c.Created).AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                query = query.Where(c => c.Name.Contains(request.SearchText) || c.RegistrationCertificateNumber.Contains(request.SearchText) || c.Code == request.SearchText || c.InstitutionTypeName.Contains(request.SearchText));
            }

            if (request.Status.HasValue)
            {
                var statusSearch = GetCustomerDomainStatus(request.Status.Value);
                query = query.Where(c => c.Status == statusSearch);
            }

            if (request.StartDate.HasValue && request.EndDate.HasValue)
            {
                query = query.Where(c => c.Created >= request.StartDate.Value && c.Created <= request.EndDate.Value);
            }

            var customers = await query.Skip((request.PageIndex.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value).AsNoTracking().ToListAsync();

            var customerCount = await query.CountAsync();

            var list = new List<CustomerExportDto>();

            foreach (var customer in customers)
            {
                var listItem = new CustomerExportDto();

                listItem.Id = customer.Id;
                listItem.Code = customer.Code;
                listItem.CustomCode = customer.CustomCode;
                listItem.Name = customer.Name;
                listItem.RegistrationCertificateNumber = customer.RegistrationCertificateNumber;
                listItem.IncorporationDate = customer.IncorporationDate.Value;
                listItem.RegisterAddress1 = customer.RegisterAddress1;
                listItem.RegisterAddress2 = customer.RegisterAddress2;
                listItem.TaxIdentificationNumber = customer.TaxIdentificationNumber;
                listItem.Country = customer.Country;
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
                //listItem.HasChildInstitutionType = customer.HasChildInstitutionType;
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
                listItem.ParentId = customer.ParentId;
                listItem.ParentName = customer.ParentName;
                listItem.ParentCode = customer.ParentCode;
                //listItem.ParentInstitutionCode = customer.Parent != null ? customer.Parent.InstitutionCode : "";

                listItem.StatusDisplay = customer.Status.ToString();
                listItem.IsLock = customer.IsLock;
                listItem.DueDiligenceCompleted = customer.DueDiligenceCompleted;

                listItem.Created = customer.Created;
                listItem.CreatedBy = customer.CreatedBy;
                listItem.LastModified = customer.LastModified;
                listItem.LastModifiedBy = customer.LastModifiedBy;

                list.Add(listItem);
            }

            result.Content = _fileBuilder.ExportData<CustomerExportDto>(list);
            result.ContentType = "text/csv";
            result.FileName = $"Customers_{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.csv";

            return await Task.FromResult(result);
        }

        private Domain.Enums.OnboardingStatus GetCustomerDomainStatus(OnboardingStatus status)
        {
            if (status == OnboardingStatus.NotStarted)
                return Domain.Enums.OnboardingStatus.NotStarted;
            else if (status == OnboardingStatus.Submitted)
                return Domain.Enums.OnboardingStatus.Submitted;
            else if (status == OnboardingStatus.Processing)
                return Domain.Enums.OnboardingStatus.Processing;
            else if (status == OnboardingStatus.Queried)
                return Domain.Enums.OnboardingStatus.Queried;
            else if (status == OnboardingStatus.Completed)
                return Domain.Enums.OnboardingStatus.Completed;
            else
                throw new NotImplementedException();
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
    }
}
