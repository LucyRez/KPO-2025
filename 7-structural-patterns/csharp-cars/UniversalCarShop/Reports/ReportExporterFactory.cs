using System;
using ReportServer.Client;

namespace UniversalCarShop.Reports;

/// <summary>
/// Фабрика для создания экспортеров отчетов.
/// </summary>
public sealed class ReportExporterFactory
{
    private readonly ReportServerClient _reportServerClient;

    public ReportExporterFactory(ReportServerClient reportServerClient)
    {
        _reportServerClient = reportServerClient;
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
            ReportFormat.Server => new ServerReportExporter(_reportServerClient), // создаем экспортер в формате Server
            _ => throw new ArgumentException($"Неизвестный формат экспорта: {format}") // выбрасываем исключение, если формат не известен
        };
    }
}
