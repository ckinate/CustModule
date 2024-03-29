using CsvHelper;
using CsvHelper.Configuration;
using Fintrak.CustomerPortal.Application.Common.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using System.Globalization;

namespace Fintrak.CustomerPortal.Infrastructure.Files
{
	public class CsvFileBuilder : ICsvFileBuilder
	{
		public byte[] ExportData<TData>(IEnumerable<TData> records)
		{
			using var memoryStream = new MemoryStream();
			using (var streamWriter = new StreamWriter(memoryStream))
			{
				using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

				csvWriter.WriteRecords(records);
			}

			return memoryStream.ToArray();
		}

		public byte[] ExportData<TData, TClassMap>(IEnumerable<TData> records) where TClassMap : ClassMap
		{
			using var memoryStream = new MemoryStream();
			using (var streamWriter = new StreamWriter(memoryStream))
			{
				using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

				csvWriter.Configuration.RegisterClassMap<TClassMap>();
				csvWriter.WriteRecords(records);
			}

			return memoryStream.ToArray();
		}

		public List<TTemplate> ImportData<TTemplate>(IBrowserFile records, long? maximumUploadSize)
		{
			Stream stream;

			if (maximumUploadSize is not null)
			{
				stream = records.OpenReadStream((Int32)maximumUploadSize);
			}
			else
			{
				stream = records.OpenReadStream();
			}

			TextReader reader = new StreamReader(stream);
			var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

			var result = csvReader.GetRecords<TTemplate>();

			stream.Close();
			stream.Dispose();

			reader.Close();

			return result.ToList();
		}

		public List<TTemplate> ImportData<TTemplate, TClassMap>(IBrowserFile records, long? maximumUploadSize) where TClassMap : ClassMap
		{
			Stream stream;

			if (maximumUploadSize is not null)
			{
				stream = records.OpenReadStream((Int32)maximumUploadSize);
			}
			else
			{
				stream = records.OpenReadStream();
			}

			TextReader reader = new StreamReader(stream);
			var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
			csvReader.Configuration.RegisterClassMap<TClassMap>();

			var result = csvReader.GetRecords<TTemplate>();

			stream.Close();
			stream.Dispose();

			reader.Close();

			return result.ToList();
		}

		public List<TTemplate> ImportData<TTemplate>(byte[] records, string contentType, long fileSize)
		{
			using MemoryStream stream = new MemoryStream(records);
			using TextReader reader = new StreamReader(stream);

			var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);

			var result = csvReader.GetRecords<TTemplate>();

			return result.ToList();
		}

		public List<TTemplate> ImportData<TTemplate, TClassMap>(byte[] records, string contentType, long fileSize) where TClassMap : ClassMap
		{
			using MemoryStream stream = new MemoryStream(records);
			using TextReader reader = new StreamReader(stream);

			var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture);
			csvReader.Configuration.RegisterClassMap<TClassMap>();
			var result = csvReader.GetRecords<TTemplate>();

			return result.ToList();
		}


	}
}
