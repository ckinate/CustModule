namespace Fintrak.CustomerPortal.Blazor.Shared.Extensions
{
	public static class NumericExtensions
	{
		// Taken from: http://stackoverflow.com/questions/2050805/getting-day-suffix-when-using-datetime-tostring 
		public static String ToOrdinal(this Int32 value)
		{
			var ordinal = "";

			switch (value)
			{
				case 1:
				case 21:
				case 31:
					ordinal = "st";
					break;
				case 2:
				case 22:
					ordinal = "nd";
					break;
				case 3:
				case 23:
					ordinal = "rd";
					break;
				default:
					ordinal = "th";
					break;
			}

			return ordinal;
		}

		public static decimal MultiplyBy(this decimal value, decimal factor)
		{
			return value * factor;
		}
	}
}
