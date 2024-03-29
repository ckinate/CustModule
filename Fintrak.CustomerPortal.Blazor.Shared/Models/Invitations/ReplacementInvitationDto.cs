namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Invitations
{
    public class ReplacementInvitationDto
    {
        public int CustomerId { get; set; }
        public string? AdminName { get; set; }
        public string? AdminEmail { get; set; }
        //Sha256Hash - CustomerId + AdminName + AdminEmail + Secret
        public string? Hash { get; set; }
    }
}
