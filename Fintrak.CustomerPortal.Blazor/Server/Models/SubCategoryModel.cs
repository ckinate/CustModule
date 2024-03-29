namespace Fintrak.CustomerPortal.Blazor.Server.Models
{
	public class SubCategoryModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public int CategoryId { get; set; }
	}
}
