using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Common.Models;
using Fintrak.CustomerPortal.Blazor.Shared.Models.Users;
using Fintrak.CustomerPortal.Infrastructure.Persistence.Migrations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Infrastructure.Identity
{
	public class IdentityService : IIdentityService
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
		private readonly IAuthorizationService _authorizationService;

		public IdentityService(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
			IAuthorizationService authorizationService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_userClaimsPrincipalFactory = userClaimsPrincipalFactory;
			_authorizationService = authorizationService;
		}

		public async Task<string> GetUserNameAsync(string userId)
		{
			var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

			return user.UserName;
		}

		public async Task<string> GetEmailAsync(string userId)
		{
			var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

			return user.Email;
		}

		public async Task<UserDto> GetUserAsync(string userId)
		{
			var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

			if(user != null)
			{
				return new UserDto
				{
					UserName = user.UserName,
					Email = user.Email,
					PhoneNumber = user.PhoneNumber,
					AdminName = user.AdminName,
					CompanyName = user.CompanyName,
					AcceptTerms = user.AcceptTerms
				};
			}

			return null;
		}

		public async Task<string> GetInvitationCodeAsync(string userId)
		{
			var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

			return user.InvitationCode;
		}

		public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
		{
			var user = new ApplicationUser
			{
				UserName = userName,
				Email = userName,
			};

			var result = await _userManager.CreateAsync(user, password);

			return (result.ToApplicationResult(), user.Id);
		}

		public async Task<(Result Result, string UserId)> ChangePasswordAsync(string userId, string oldPassword, string newPassword)
		{
			var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
			if (user == null)
			{
				throw new Exception($"Unable to load user with ID '{userId}'.");
			}

			var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
			if (!changePasswordResult.Succeeded)
			{
				return (changePasswordResult.ToApplicationResult(), user.Id);
			}

			await _signInManager.RefreshSignInAsync(user);

			return (changePasswordResult.ToApplicationResult(), user.Id);
		}

		public async Task<bool> IsInRoleAsync(string userId, string role)
		{
			var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

			return user != null && await _userManager.IsInRoleAsync(user, role);
		}

		public async Task<bool> AuthorizeAsync(string userId, string policyName)
		{
			var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

			if (user == null)
			{
				return false;
			}

			var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

			var result = await _authorizationService.AuthorizeAsync(principal, policyName);

			return result.Succeeded;
		}

		public async Task<Result> DeleteUserAsync(string userId)
		{
			var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

			return user != null ? await DeleteUserAsync(user) : Result.Success();
		}

		public async Task<Result> DeleteUserAsync(ApplicationUser user)
		{
			var result = await _userManager.DeleteAsync(user);

			return result.ToApplicationResult();
		}

		public async Task<bool> LockUserAsync(string userId, bool lockFlag)
		{
			var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

			if (lockFlag)
			{
				var lockUserResult = await _userManager.SetLockoutEnabledAsync(user, true);
				var lockDateResult = await _userManager.SetLockoutEndDateAsync(user, new DateTime(2222, 06, 06));

				return lockUserResult.Succeeded && lockDateResult.Succeeded;
			}
			else
			{
				var lockDisabledResult = await _userManager.SetLockoutEnabledAsync(user, false);
				//var setLockoutEndDateResult = await _userManager.SetLockoutEndDateAsync(user, DateTime.Now - TimeSpan.FromMinutes(1));

				return lockDisabledResult.Succeeded;// && setLockoutEndDateResult.Succeeded;
			}			
		}

        public async Task<bool> AcceptTermsAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);
            if (user == null)
            {
                return false;
            }

			user.AcceptTerms = true;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
