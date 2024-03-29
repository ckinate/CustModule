namespace Fintrak.CustomerPortal.Application.Utilities
{
	public static class FileUtility
	{
		public static string GetFileExtension(string contentType)
		{
			if (contentType.Contains("text/csv"))
			{
				return "csv";
			}
			else if (contentType.Contains("image/svg+xml"))
			{
				return "svg";
			}
			else if (contentType.Contains("image/png"))
			{
				return "png";
			}
			else if (contentType.Contains("image/gif"))
			{
				return "gif";
			}
			else if (contentType.Contains("image/bmp"))
			{
				return "bmp";
			}
			else if (contentType.Contains("image/jpeg"))
			{
				return "jpeg";
			}
			else if (contentType.Contains("text/plain"))
			{
				return "txt";
			}
			else if (contentType.Contains("text/html"))
			{
				return "html";
			}
			else if (contentType.Contains("application/msword"))
			{
				return "doc";
			}
			else if (contentType.Contains("application/pdf"))
			{
				return "pdf";
			}
			else if (contentType.Contains("application/vnd.ms-powerpoint"))
			{
				return "ppt";
			}
			else if (contentType.Contains("application/vnd.openxmlformats-officedocument.presentationml.presentation"))
			{
				return "pptx";
			}
			else if (contentType.Contains("application/msword"))
			{
				return "doc";
			}
			else if (contentType.Contains("application/vnd.openxmlformats-officedocument.wordprocessingml.document"))
			{
				return "docx";
			}
			else if (contentType.Contains("application/vnd.ms-excel"))
			{
				return "xls";
			}
			else if (contentType.Contains("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
			{
				return "xlsx";
			}
			else if (contentType.Contains("video/x-msvideo"))
			{
				return "avi";
			}
			else if (contentType.Contains("audio/wav"))
			{
				return "wav";
			}
			else if (contentType.Contains("audio/mpeg"))
			{
				return "mp3";
			}
			else if (contentType.Contains("application/vnd.rar"))
			{
				return "rar";
			}
			else if (contentType.Contains("application/zip"))
			{
				return "zip";
			}
			else if (contentType.Contains("application/x-7z-compressed"))
			{
				return "7z";
			}
			else if (contentType.Contains("application/rtf"))
			{
				return "rtf";
			}

			return "";
		}

		public static string GetFileSize(long byteValue, FileSize fileSize, bool includeUnit = true)
		{
			string result = string.Empty;
			long calculatedSize;

			if (fileSize == FileSize.B)
			{
				calculatedSize = byteValue;
				result = includeUnit ? $"{calculatedSize}{fileSize}" : calculatedSize.ToString();
			}
			else if (fileSize == FileSize.KB)
			{
				calculatedSize = byteValue / 1024;
				result = includeUnit ? $"{calculatedSize}{fileSize}" : calculatedSize.ToString();
			}
			else if (fileSize == FileSize.MB)
			{
				calculatedSize = byteValue / (1024 * 1024);
				result = includeUnit ? $"{calculatedSize}{fileSize}" : calculatedSize.ToString();
			}
			else if (fileSize == FileSize.GB)
			{
				calculatedSize = byteValue / (1024 * 1024 * 1024);
				result = includeUnit ? $"{calculatedSize}{fileSize}" : calculatedSize.ToString();
			}

			return result;
		}

		public enum FileSize
		{
			B = 1,
			KB = 2,
			MB = 3,
			GB = 4,
			TB = 5
		}
	}
}
