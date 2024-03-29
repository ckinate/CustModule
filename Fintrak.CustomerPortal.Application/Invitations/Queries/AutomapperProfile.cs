using AutoMapper;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Invitations;
using Fintrak.CustomerPortal.Domain.Entities;

namespace Fintrak.InvitationPortal.Application.Invitations.Queries;
public class AutomapperProfile : Profile
{
	public AutomapperProfile()
	{
		CreateMap<Invitation, InvitationDto>();
	}
}

