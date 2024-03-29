using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Fintrak.CustomerPortal.Application.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fintrak.CustomerPortal.Infrastructure.Files
{
	public class FileStore : IFileStore
	{
		private readonly IHostingEnvironment _environment;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public FileStore(IHostingEnvironment environment, IHttpContextAccessor httpContextAccessor)
		{
			_environment = environment;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task DeleteFile(string locationUrl, string folder)
		{
			var directory = Path.Combine(_environment.WebRootPath, $"uploads/{folder}");
			var path = Path.Combine(directory, locationUrl);

			var file = new FileInfo(path);
			if (file.Exists)
				file.Delete();

			await Task.CompletedTask;
		}

		public async Task<string> UploadFile(byte[] fileData, string identifier, string contentType, string folder)
		{
			string fileExtenstion = FileUtility.GetFileExtension(contentType);
			string fileName = $"{identifier}.{fileExtenstion}";

			var directory = Path.Combine(_environment.WebRootPath, $"Uploads/{folder}");
			if (!Directory.Exists(directory))
				Directory.CreateDirectory(directory);

			var path = Path.Combine(directory, fileName);

			using (var fileStream = File.Create(path))
			{
				await fileStream.WriteAsync(fileData);
			}

			var url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/uploads/{folder}/{fileName}";
			return url;
		}
	}
}
