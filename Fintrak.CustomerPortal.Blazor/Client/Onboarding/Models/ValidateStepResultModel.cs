namespace Fintrak.CustomerPortal.Blazor.Client.Onboarding.Models
{
	public class ValidateStepResultModel
	{
		public bool Valid { get; set; }
		public List<string> Errors { get; set; } = new();
	}
}
