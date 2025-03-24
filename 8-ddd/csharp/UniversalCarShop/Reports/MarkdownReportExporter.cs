using System;

namespace UniversalCarShop.Reports;

/// <summary>
/// Класс для экспорта отчета в формате Markdown.
/// </summary>
public sealed class MarkdownReportExporter : ReportExporter
{
    /// <inheritdoc/>
    public override void Export(Report report, TextWriter writer)
    {
        writer.WriteLine($"# {report.Title}");
        writer.WriteLine();
        writer.WriteLine(report.Content);
    }
}
