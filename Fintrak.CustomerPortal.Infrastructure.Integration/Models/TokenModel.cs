using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Infrastructure.Integration.Models
{
    public class GetTokenInput
    {
        public string LoginId { get; set; }
		public string Password { get; set; }
	}


	public class TokenModel
    {
        public TokenResultModel Result { get; set; }
        public string TargetUrl { get; set; }
        public bool Success { get; set; }
        public string Error { get; set; }
    }

    public class TokenResultModel
    {
        public string AccessToken { get; set; }
        public string EncryptedAccessToken { get; set; }
        public int ExpireInSeconds { get; set; }
        public bool ShouldResetPassword { get; set; }
        public string PasswordResetCode { get; set; }
        public int UserId { get; set; }
        public bool RequiresTwoFactorVerification { get; set; }
        public string TwoFactorAuthProviders { get; set; }
        public string TwoFactorRememberClientToken { get; set; }
        public string ReturnUrl { get; set; }
        public string RefreshToken { get; set; }
        public int RefreshTokenExpireInSeconds { get; set; }
    }
}
