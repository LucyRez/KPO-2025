using System;
using System.Text.Json;

namespace UniversalCarShop.Reports;

/// <summary>
/// Класс для экспорта отчета в формате JSON.
/// </summary>
public sealed class JsonReportExporter : ReportExporter
{
    /// <inheritdoc/>
    public override void Export(Report report, TextWriter writer)
    {
        writer.Write(JsonSerializer.Serialize(report));
    }
}
