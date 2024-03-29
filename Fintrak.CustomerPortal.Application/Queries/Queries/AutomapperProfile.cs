using AutoMapper;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Enums;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Queries;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.InvitationPortal.Application.Queries.Queries;
public class AutomapperProfile : Profile
{
	public AutomapperProfile()
	{
		CreateMap<Query, QueryDto>()
			.ForMember(c => c.ResourceType, option => option.MapFrom(c => GetSharedResourceType(c.ResourceType)))
			.ForMember(c => c.PreviousStatus, option => option.MapFrom(c => GetCustomerStatus(c.PreviousStatus)))
			.ForMember(c => c.CustomerCode, option => option.MapFrom(c => c.Customer.Code))
			.ForMember(c => c.CustomerName, option => option.MapFrom(c => c.Customer.Name));
	}

	private ResourceType GetSharedResourceType(CustomerPortal.Domain.Enums.ResourceType resourceType)
	{
		if (resourceType == CustomerPortal.Domain.Enums.ResourceType.Customer)
		{
			return ResourceType.Customer;
		}
		else
		{
			throw new NotImplementedException("ResourceType not implemented.");
		}
	}

	private OnboardingStatus GetCustomerStatus(CustomerPortal.Domain.Enums.OnboardingStatus status)
	{
		if (status == CustomerPortal.Domain.Enums.OnboardingStatus.NotStarted)
			return OnboardingStatus.NotStarted;
		else if (status == CustomerPortal.Domain.Enums.OnboardingStatus.Submitted)
			return OnboardingStatus.Submitted;
		else if (status == CustomerPortal.Domain.Enums.OnboardingStatus.Processing)
			return OnboardingStatus.Processing;
		else if (status == CustomerPortal.Domain.Enums.OnboardingStatus.Queried)
			return OnboardingStatus.Queried;
		else if (status == CustomerPortal.Domain.Enums.OnboardingStatus.Completed)
			return OnboardingStatus.Completed;
		else
			throw new NotImplementedException();
	}
}

