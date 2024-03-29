namespace Fintrak.CustomerPortal.Application.Common.Interfaces
{
    public interface IExcelFileBuilder
    {
        byte[] DownloadUploadTemplate<TTemplate>(string templateTitle);
    }
}
