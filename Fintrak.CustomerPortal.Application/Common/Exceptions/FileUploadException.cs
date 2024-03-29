namespace Fintrak.CustomerPortal.Application.Common.Exceptions;

public class FileUploadException : Exception
{
    public FileUploadException()
        : base()
    {
    }

    public FileUploadException(string message)
        : base(message)
    {
    }

    public FileUploadException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public FileUploadException(string fileTitle, object fileOwner)
        : base($"Fail to upload file: \"{fileTitle}\" for {fileOwner}.")
    {
    }
}
