namespace Fintrak.CustomerPortal.Blazor.Shared.Models.Queries
{
	public class ResponseToQueryDto
	{
		public int QueryId { get; set; }
		public string Response { get; set; }
        public bool RequireDataModification { get; set; }
    }
}
