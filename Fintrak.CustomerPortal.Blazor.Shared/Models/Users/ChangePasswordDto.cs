﻿namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Users
{
	public class ChangePasswordDto
	{
		public string? OldPassword { get; set; }
		public string? NewPassword { get; set; }
		public string? ConfirmPassword { get; set; }
	}
}