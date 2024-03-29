
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Components.Forms;

namespace Fintrak.CustomerPortal.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
    byte[] ExportData<TData>(IEnumerable<TData> records);

    byte[] ExportData<TData, TClassMap>(IEnumerable<TData> records) where TClassMap : ClassMap;

    List<TTemplate> ImportData<TTemplate>(IBrowserFile records, long? maximumUploadSize);

    List<TTemplate> ImportData<TTemplate, TClassMap>(IBrowserFile records, long? maximumUploadSize) where TClassMap : ClassMap;

    List<TTemplate> ImportData<TTemplate>(byte[] records, string contentType, long fileSize);

    List<TTemplate> ImportData<TTemplate, TClassMap>(byte[] records, string contentType, long fileSize) where TClassMap : ClassMap;
}

