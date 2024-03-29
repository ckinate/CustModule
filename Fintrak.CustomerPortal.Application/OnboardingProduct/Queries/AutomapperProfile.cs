using AutoMapper;
using Fintrak.CustomerPortal.Application.Common.Extensions;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Onboarding;
using Fintrak.CustomerPortal.Blazor.Shared.Models.OnboardingProduct;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.CustomerPortal.Application.OnboardingProduct.Queries
{
	public class AutomapperProfile : Profile
	{
		public AutomapperProfile()
		{
			CreateMap<CustomerProduct, CustomerProductDto>()
				//.ForMember(c => c.ContactPersonId, option => option.MapFrom(c => c.CustomerContactPersonId))
				//.ForMember(c => c.ContactPersonName, option => option.MapFrom(c => c.CustomerContactPerson.Name))
				//.ForMember(c => c.AccountId, option => option.MapFrom(c => c.CustomerAccountId))
				//.ForMember(c => c.AccountNumber, option => option.MapFrom(c => c.CustomerAccount.AccountNumber))
				.ForMember(c => c.Status,option => option.MapFrom(c => c.Status.GetProductStatus()))
				.ForMember(c => c.StatusDisplay,option => option.MapFrom(c => c.Status.GetProductStatusDisplay()));

			CreateMap<CustomerProduct, OnboardProductDto>()
				.ForMember(c => c.CustomerCode,option => option.MapFrom(c => c.Customer.Code))
				.ForMember(c => c.CustomerName, option => option.MapFrom(c => c.Customer.Name))
				.ForMember(c => c.ContactPersonId, option => option.MapFrom(c => c.CustomerContactPersonId))
				.ForMember(c => c.ContactPersonName, option => option.MapFrom(c => c.CustomerContactPerson.Name))
				.ForMember(c => c.AccountId, option => option.MapFrom(c => c.CustomerAccountId))
				.ForMember(c => c.AccountNumber, option => option.MapFrom(c => c.CustomerAccount.AccountNumber))
				.ForMember(c => c.Status,option => option.MapFrom(c => c.Status.GetProductStatus()));

			//CreateMap<CustomerSignatory, CustomerSignatoryDto>();

			CreateMap<CustomerProductDocument, DocumentDto>()
				.ForMember(c => c.FileData, o => o.Ignore())
				.ForMember(c => c.ContentType, o => o.Ignore())
				.ForMember(c => c.Size, o => o.Ignore());
		}
	}

}
