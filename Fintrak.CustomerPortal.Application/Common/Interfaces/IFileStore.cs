namespace Fintrak.CustomerPortal.Application.Common.Interfaces
{
	public interface IFileStore
	{
		Task<string> UploadFile(byte[] fileData, string identifier, string contentType, string folder);
		Task DeleteFile(string locationUrl, string folder);
	}
}
