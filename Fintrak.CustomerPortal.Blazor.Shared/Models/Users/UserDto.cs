namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Users
{
	public class UserDto
	{
		public string? UserName { get; set; }
		public string? Email { get; set; }
		public string? PhoneNumber { get; set; }
		public string? AdminName { get; set; }
		public string? CompanyName { get; set; }
        public bool? AcceptTerms { get; set; }
    }
}
