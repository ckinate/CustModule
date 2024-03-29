namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Invitations
{
	public class CreateInvitationDto
	{
		public string? CompanyName { get; set; }
		public string? AdminName { get; set; }
		public string? AdminEmail { get; set; }
		//Sha256Hash - CompanyName + AdminName + AdminEmail + Secret
		public string? Hash { get; set; }

    }
}
