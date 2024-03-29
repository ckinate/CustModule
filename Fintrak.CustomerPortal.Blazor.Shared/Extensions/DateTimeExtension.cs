using System.Globalization;
using System.Text;

namespace Fintrak.CustomerPortal.Blazor.Shared.Extensions
{
	public static class DateTimeExtension
	{
		public static DateTime FirstDayOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1);
		}

		public static DateTime LastDayOfMonth(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, 1).AddMonths(1).AddDays(-1);
		}

		public static DateTime FirstDayOfWeek(this DateTime date)
		{
			DayOfWeek fdow = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;


			int offset = fdow - date.DayOfWeek;
			DateTime fdowDate = date.AddDays(offset);
			return fdowDate;
		}

		public static DateTime LastDayOfWeek(this DateTime date)
		{
			DateTime ldowDate = FirstDayOfWeek(date).AddDays(6);
			return ldowDate;
		}

		public static DateTime MorningTime(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, date.Day, 0, 0, 59);
		}

		public static DateTime NightTime(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
		}

		public static DateTime LastWeek(this DateTime date, DayOfWeek weekStart)
		{
			DateTime startingDate = date;

			while (startingDate.DayOfWeek != weekStart)
				startingDate = startingDate.AddDays(-1);

			DateTime previousWeekStart = startingDate.AddDays(-7);
			DateTime previousWeekEnd = startingDate.AddDays(-1);

			return previousWeekEnd;
		}

		public static DateTime LastWeek(this DateTime? date, DayOfWeek weekStart)
		{
			DateTime startingDate = date.Value;

			while (startingDate.DayOfWeek != weekStart)
				startingDate = startingDate.AddDays(-1);

			DateTime previousWeekStart = startingDate.AddDays(-7);
			DateTime previousWeekEnd = startingDate.AddDays(-1);

			return previousWeekEnd;
		}

		public static int Quarter(this DateTime date)
		{
			return (date.Month + 2) / 3;
		}

		//public static int FinancialQuarter(this DateTime date)
		//{
		//    return (date.AddMonths(-3).Month + 2) / 3;
		//}

		public static int MidYear(this DateTime date)
		{
			return date.Month <= 6 ? 1 : 2;
		}

		/// <summary>
		/// Compares a supplied date to the current date and generates a friendly English 
		/// comparison ("5 days ago", "5 days from now")
		/// </summary>
		/// <param name="date">The date to convert</param>
		/// <param name="approximate">When off, calculate timespan down to the second.
		/// When on, approximate to the largest round unit of time.</param>
		/// <returns></returns>
		public static string ToRelativeDateString(this DateTime value, bool approximate)
		{
			StringBuilder sb = new StringBuilder();

			string suffix = (value > DateTime.Now) ? " from now" : " ago";

			TimeSpan timeSpan = new TimeSpan(Math.Abs(DateTime.Now.Subtract(value).Ticks));

			if (timeSpan.Days > 0)
			{
				sb.AppendFormat("{0} {1}", timeSpan.Days,
				  (timeSpan.Days > 1) ? "days" : "day");
				if (approximate) return sb.ToString() + suffix;
			}

			if (timeSpan.Hours > 0)
			{
				sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
				  timeSpan.Hours, (timeSpan.Hours > 1) ? "hours" : "hour");
				if (approximate) return sb.ToString() + suffix;
			}

			if (timeSpan.Minutes > 0)
			{
				sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
				  timeSpan.Minutes, (timeSpan.Minutes > 1) ? "minutes" : "minute");
				if (approximate) return sb.ToString() + suffix;
			}

			if (timeSpan.Seconds > 0)
			{
				sb.AppendFormat("{0}{1} {2}", (sb.Length > 0) ? ", " : string.Empty,
				  timeSpan.Seconds, (timeSpan.Seconds > 1) ? "seconds" : "second");
				if (approximate) return sb.ToString() + suffix;
			}

			if (sb.Length == 0) return "right now";

			sb.Append(suffix);

			return sb.ToString();
		}

		public static String ToRelativeFormat(this DateTime dateTime)
		{
			var now = DateTime.Now;
			var timeDifference = now - dateTime;
			var daySuffix = GetDaySuffix(dateTime);

			if (now.Year == dateTime.Year)
			{
				// exclude year

				if (now.Month == dateTime.Month)
				{
					// exclude month

					if (now.Day == dateTime.Day)
					{
						// exclude day

						if (timeDifference.Hours < 24)
						{
							// display as hours

							if (timeDifference.Hours < 1)
							{
								// display as minutes

								if (timeDifference.Minutes < 1)
								{
									// display as seconds
									if (timeDifference.Seconds <= 1)
										return "Just now";

									return timeDifference.Seconds + " seconds ago";
								}
								if (timeDifference.Minutes == 1)
								{
									return timeDifference.Minutes + " minute ago";
								}

								// display as minutes
								return timeDifference.Minutes + " minutes ago";
							}

							// display as hours
							if (timeDifference.Hours == 1)
								return "1 hour ago";

							return timeDifference.Hours + " hours ago";
						}
					}

					// display with year and month excluded
					return dateTime.ToString($"dddd, d'{daySuffix}' 'at' h:mm tt");
				}

				// display with year excluded
				return dateTime.ToString($"dddd, MMMM d'{daySuffix}' 'at' h:mm tt");
			}

			// display with name of day excluded
			return dateTime.ToString($"MMMM d'{daySuffix}', yyyy 'at' h:mm tt");
		}

		/// <summary>
		/// Converts a given DateTime into a Unix timestamp
		/// </summary>
		/// <param name="value">Any DateTime</param>
		/// <returns>The given DateTime in Unix timestamp format</returns>
		public static long ToUnixTimestamp(this DateTime value)
		{
			return (long)(value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		}

		/// <summary>
		/// Gets a Unix timestamp representing the current moment
		/// </summary>
		/// <param name="ignored">Parameter ignored</param>
		/// <returns>Now expressed as a Unix timestamp</returns>
		public static long UnixTimestamp(this DateTime ignored)
		{
			return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
		}

		/// <summary>
		/// Returns a local DateTime based on provided unix timestamp
		/// </summary>
		/// <param name="timestamp">Unix/posix timestamp</param>
		/// <returns>Local datetime</returns>
		public static DateTime ParseUnixTimestamp(long timestamp)
		{
			return (new DateTime(1970, 1, 1)).AddSeconds(timestamp).ToLocalTime();
		}


		#region HELPERS


		private static String GetDaySuffix(DateTime date)
		{
			return date.Day.ToOrdinal();
		}

		#endregion
	}
}
