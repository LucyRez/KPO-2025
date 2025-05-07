using System;
using UniversalCarShop.Entities.Common;
using UniversalCarShop.UseCases.Reports;

namespace UniversalCarShop.UseCases.Reports;

/// <summary>
/// Класс для экспорта отчета в формате Markdown.
/// </summary>
internal sealed class MarkdownReportExporter : ReportExporter
{
    /// <inheritdoc/>
    public override void Export(Report report, TextWriter writer)
    {
        writer.WriteLine($"# {report.Title}");
        writer.WriteLine();
        writer.WriteLine(report.Content);
    }
}
