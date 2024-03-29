namespace Fintrak.CustomerPortal.Blazor.Shared.Models
{
    public class ExportDto
    {
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}
