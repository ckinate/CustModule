using PhoneNumbers;

namespace Fintrak.CustomerPortal.Blazor.Client
{
	public static class PhoneValidator
	{
		public static bool ValidatePhoneNumber(string callCode, string phoneNumber)
		{
			PhoneNumberUtil phoneUtil = PhoneNumberUtil.GetInstance();

			try
			{
				if (string.IsNullOrEmpty(callCode) || string.IsNullOrEmpty(phoneNumber))
					return false;

				if (callCode == "NG" && (phoneNumber.Length != 8 && phoneNumber.Length != 11))
					return false;

				PhoneNumbers.PhoneNumber phoneNumberValidationResult = phoneUtil.Parse(phoneNumber, callCode);
				bool isValidNumber = phoneUtil.IsValidNumber(phoneNumberValidationResult);
				return isValidNumber;
			}
			catch (NumberParseException ex)
			{
				String errorMessage = "NumberParseException was thrown: " + ex.Message.ToString();
			}

			return false;
		}
	}
}
