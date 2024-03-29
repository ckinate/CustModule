// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Blazor.Client.Invitations;
using Fintrak.CustomerPortal.Blazor.Server.Services;
using Fintrak.CustomerPortal.Infrastructure.Identity;
using Fintrak.CustomerPortal.Infrastructure.Persistence.Migrations;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Fintrak.CustomerPortal.Blazor.Server.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailService _emailSender;
		private readonly IRegisterService _registerService;
		private readonly IConfiguration _configuration;

		public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
			IEmailService emailSender,
            IRegisterService registerService,
			IConfiguration configuration)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
			_registerService = registerService;
            _configuration = configuration;

		}

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

		public string InvitationCode { get; set; }

        public bool ReplaceAdmin { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
			[Required]
			[Display(Name = "Company Name")]
			public string CompanyName { get; set; }

			[Required]
			[Display(Name = "Admin Name")]
			public string AdminName { get; set; }

			//[Required]
			[Display(Name = "Invitation Code")]
			public string InvitationCode { get; set; }

            [Display(Name = "Replace Admin")]
            public bool ReplaceAdmin { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Admin Email")]
            public string AdminEmail { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string invitationCode = null, bool replacement = false, string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            InvitationCode = invitationCode;
            ReplaceAdmin = replacement;

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (!string.IsNullOrEmpty(invitationCode))
            {
				var invitationResponse = await _registerService.GetInvitation(invitationCode);
                if(invitationResponse != null && invitationResponse.Success && invitationResponse.Result != null && !invitationResponse.Result.Used)
                {
                    Input = new InputModel();

					Input.CompanyName = invitationResponse.Result.CompanyName;
					Input.AdminName = invitationResponse.Result.AdminName;
					Input.AdminEmail = invitationResponse.Result.AdminEmail;
                    Input.ReplaceAdmin = replacement;
					Input.InvitationCode = invitationCode.ToString();
				}
			}         
		}

        public async Task<IActionResult> OnPostAsync(string invitationCode = null, bool replacement = false, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            try
            {
				ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
				if (ModelState.IsValid)
				{
					var user = CreateUser();

					await _userStore.SetUserNameAsync(user, Input.AdminEmail, CancellationToken.None);
					await _emailStore.SetEmailAsync(user, Input.AdminEmail, CancellationToken.None);

					user.CompanyName = Input.CompanyName;
					user.AdminName = Input.AdminName;
					user.InvitationCode = Input.InvitationCode;

					var result = await _userManager.CreateAsync(user, Input.Password);
					if (result.Succeeded)
					{
						_logger.LogInformation("User created a new account with password.");

						//Update invitation
						if (!string.IsNullOrEmpty(Input.InvitationCode))
						{
							var invitationResponse = await _registerService.GetInvitation(Input.InvitationCode);
							if (invitationResponse != null &&
								invitationResponse.Success && invitationResponse.Result != null && !invitationResponse.Result.Used)
							{
								await _registerService.UseInvitation(Input.InvitationCode, user.Id);
							}
						}

						var userId = await _userManager.GetUserIdAsync(user);
						var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
						code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
						var callbackUrl = Url.Page(
							"/Account/ConfirmEmail",
							pageHandler: null,
							values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
							protocol: Request.Scheme);

						//Template placeholder
						//[[PreHeaderText]], [[CompanyName]], [[CallackLink]], [[FooterInfo]]

						//https://localhost:7266/Identity/Account/Register?invitationCode=TestCode&returnUrl=
						//var portalBaseUrl = _configuration["PortalUrl"];
						//var callBackUrl = $"{portalBaseUrl}/Identity/Account/Register?invitationCode={notification.Item.Code}&returnUrl=";
						BodyBuilder template = _emailSender.GetEmailTemplateBody("customer-confirmation-email");
						//var body = string.Format(template.HtmlBody, notification.Item.CompanyName, notification.Item.AdminName, notification.Item.AdminEmail, callBackUrl);

						var body = template.HtmlBody.Replace("[[PreHeaderText]]", "");
                        body = body.Replace("[[Salutation]]", $"Dear {user.AdminName}");
                        //body = body.Replace("[[CompanyName]]", notification.Item.CompanyName);
                        //body = body.Replace("[[AdminName]]", notification.Item.AdminName);
                        //body = body.Replace("[[AdminEmail]]", notification.Item.AdminEmail);
                        body = body.Replace("[[CallackLink]]", HtmlEncoder.Default.Encode(callbackUrl));
						body = body.Replace("[[FooterInfo]]", "NIBSS, Plot 1230, Ahmadu Bello Way, Bar Beach, Victoria Island, P. M. B. 12617, Lagos.");

						//var body = $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.";

						await _emailSender.SendEmailAsync(Input.AdminName, Input.AdminEmail, "NIBSS Customer Portal Confirmation Email", body);

						if (_userManager.Options.SignIn.RequireConfirmedAccount)
						{
							return RedirectToPage("RegisterConfirmation", new { email = Input.AdminEmail, returnUrl = returnUrl });
						}
						else
						{
							await _signInManager.SignInAsync(user, isPersistent: false);
							return LocalRedirect(returnUrl);
						}
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
			}
            catch (Exception ex) 
            { 

            }
            

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
