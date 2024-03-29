namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Invitations
{
	public class InvitationDto : BaseDto
	{
		public string? Code { get; set; }
		public string? CompanyName { get; set; }
		public string? AdminName { get; set; }
		public string? AdminEmail { get; set; }
        public DateTime? EntryDate { get; set; }
        public bool Used { get; set; }
        public DateTime? UsedDate { get; set; }
    }
}
