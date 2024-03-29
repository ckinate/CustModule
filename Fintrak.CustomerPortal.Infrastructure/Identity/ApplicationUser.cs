using Microsoft.AspNetCore.Identity;

namespace Fintrak.CustomerPortal.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
	public string? AdminName { get; set; }
	public string? CompanyName { get; set; }
	public string? InvitationCode { get; set; }
    public bool? AcceptTerms { get; set; }
}
