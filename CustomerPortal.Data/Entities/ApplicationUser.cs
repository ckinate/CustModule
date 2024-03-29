using Microsoft.AspNetCore.Identity;

namespace CustomerPortal.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public bool AgreeToTerms { get; set; }
        public bool RequirePasswordReset { get; set; }
	}
}
