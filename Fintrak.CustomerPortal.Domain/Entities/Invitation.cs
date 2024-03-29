using System.Numerics;

namespace Fintrak.CustomerPortal.Domain.Entities
{
	public class Invitation : BaseAuditableEntity
	{
		public string? Code { get; set; }
		public string? CompanyName { get; set; }
		public string? AdminName { get; set; }
		public string? AdminEmail { get; set; }
        public DateTime? EntryDate { get; set; }
        public bool Used { get; set; }
        public DateTime? UsedDate { get; set; }

        public bool ReplaceAdmin { get; set; }
    }
}
