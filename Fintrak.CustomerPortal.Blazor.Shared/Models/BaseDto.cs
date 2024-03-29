namespace Fintrak.CustomerPortal.Blazor.Shared.Models
{
    public abstract class BaseDto
    {
        public int? Id { get; set; }
        public DateTime Created { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
