using Microsoft.AspNetCore.Components.Forms;

namespace Fintrak.VendorPortal.Blazor.Client.Components
{
	public class CustomInputDateNullable<TValue> : InputDate<TValue>
	{
		protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
		{
			if (typeof(TValue) == typeof(DateTime?))
			{
				if (DateTime.TryParse(value, out var resultDate))
				{
					result = (TValue)(object)resultDate;
					validationErrorMessage = null;
					return true;
				}
				else
				{
					result = default;
					validationErrorMessage = $"The selected value {value} is not a valid boolean.";
					return false;
				}
			}
			else
			{
				return base.TryParseValueFromString(value, out result,
					out validationErrorMessage);
			}
		}

		protected override string FormatValueAsString(TValue? value)
		{
			if (value == null)
				return "";
			else
				return base.FormatValueAsString(value);
		}
	}
}
