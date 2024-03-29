using System.Collections;

namespace Fintrak.CustomerPortal.Blazor.Shared.Models
{
	public class LookupModel<TText, TValue>
	{
		public TText? Text { get; set; }
		public TValue? Value { get; set; }

		public TText? AlternateText { get; set; }
		public TText? AlternateText2 { get; set; }

		public bool HasAdditionalData { get; set; }
		public Dictionary<string,string> AdditionalData { get; set; } = new();
	}

	public class LookupModel : LookupModel<string, string>
	{
		public bool Selected { get; set; }
	}
}
