using Fintrak.CustomerPortal.Application.Common.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;

namespace Fintrak.CustomerPortal.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<string> GetUserNameAsync(string userId);

    Task<string> GetEmailAsync(string userId);

    Task<UserDto> GetUserAsync(string userId);

	Task<string> GetInvitationCodeAsync(string userId);

	Task<bool> IsInRoleAsync(string userId, string role);

    Task<bool> AuthorizeAsync(string userId, string policyName);

    Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

    Task<(Result Result, string UserId)> ChangePasswordAsync(string userId, string oldPassword, string newPassword);

	Task<Result> DeleteUserAsync(string userId);

    Task<bool> LockUserAsync(string userId, bool lockFlag);

    Task<bool> AcceptTermsAsync(string userId);
}
