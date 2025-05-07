using System;
using UniversalCarShop.Entities.Common;
using UniversalCarShop.UseCases.Reports;

namespace UniversalCarShop.UseCases.Reports;

/// <summary>
/// Фабрика для создания экспортеров отчетов.
/// </summary>
internal sealed class ReportExporterFactory : IReportExporterFactory
{
    private readonly IReportServerConnector _reportServerConnector;

    public ReportExporterFactory(IReportServerConnector reportServerConnector)
    {
        _reportServerConnector = reportServerConnector;
    }

    /// <summary>
    /// Создает экспортер отчета в зависимости от формата.
    /// </summary>
    public ReportExporter Create(ReportFormat format)
    {
        return format switch
        {
            ReportFormat.Json => new JsonReportExporter(), // создаем экспортер в формате JSON
            ReportFormat.Markdown => new MarkdownReportExporter(), // создаем экспортер в формате Markdown
            ReportFormat.Server => new ServerReportExporter(_reportServerConnector), // создаем экспортер в формате Server
            _ => throw new ArgumentException($"Неизвестный формат экспорта: {format}") // выбрасываем исключение, если формат не известен
        };
    }
}
