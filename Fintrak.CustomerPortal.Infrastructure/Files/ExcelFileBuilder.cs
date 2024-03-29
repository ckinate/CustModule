using Fintrak.CustomerPortal.Application.Common.Interfaces;
using OfficeOpenXml;

namespace Fintrak.CustomerPortal.Infrastructure.Files
{
	public class ExcelFileBuilder : IExcelFileBuilder
	{
		public byte[] DownloadUploadTemplate<TTemplate>(string templateTitle)
		{
			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

			byte[] reportBytes;
			using (var package = new ExcelPackage())
			{
				package.Workbook.Properties.Title = templateTitle;
				package.Workbook.Properties.Subject = templateTitle;
				package.Workbook.Properties.Keywords = templateTitle;

				var worksheet = package.Workbook.Worksheets.Add(templateTitle);

				var template = new List<TTemplate>();
				worksheet.Cells.LoadFromCollection(template, true);

				reportBytes = package.GetAsByteArray();
			}

			return reportBytes;
		}

	}
}
