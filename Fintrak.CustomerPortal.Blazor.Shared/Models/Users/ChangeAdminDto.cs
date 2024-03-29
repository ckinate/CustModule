namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Users
{
    public class ChangeAdminDto
    {
        public int CustomerId { get; set; }
        public string? AdminName { get; set; }
        public string? AdminEmail { get; set; }
        //Sha256Hash - CustomerId + AdminName + AdminEmail + Secret
        public string? Hash { get; set; }
    }
}
