using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Application.Onboarding.Queries;
public class AutomapperProfile : Profile
{
	public AutomapperProfile()
	{
		CreateMap<Customer, CustomerDto>()
			.ForMember(c => c.Status, option => option.MapFrom(c => c.Status.GetCustomerStatus()))
			.ForMember(c => c.StatusDisplay, option => option.MapFrom(c => c.Status.GetCustomerStatusDisplay()));
			//.ForMember(c => c.ParentCode, option => option.MapFrom(c => c.Parent.Code))
			//.ForMember(c => c.ParentName, option => option.MapFrom(c => c.Parent.Name))
			//.ForMember(c => c.ParentInstitutionCode, option => option.MapFrom(c => c.Parent.InstitutionCode));

		CreateMap<Customer, BasicCustomerDto>().ReverseMap();

		CreateMap<CustomerContactPerson, ContactPersonDto>();

		CreateMap<CustomerContactChannel, ContactChannelDto>()
			.ForMember(c=> c.Type, 
			option=> option.MapFrom(c=> c.ChannelType == Domain.Enums.ChannelType.Email ? ChannelType.Email : ChannelType.Phone));

		CreateMap<CustomerAccount, CustomerAccountDto>();

		CreateMap<CustomerSignatory, CustomerSignatoryDto>();

		CreateMap<CustomerDocument, DocumentDto>()
			.ForMember(c=>c.FileData, o=> o.Ignore())
			.ForMember(c => c.ContentType, o => o.Ignore())
			.ForMember(c => c.Size, o => o.Ignore());
	}
}

