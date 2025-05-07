using System;
using System.Text.Json;
using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.UseCases.Reports;

/// <summary>
/// Класс для экспорта отчета в формате JSON.
/// </summary>
internal sealed class JsonReportExporter : ReportExporter
{
    /// <inheritdoc/>
    public override void Export(Report report, TextWriter writer)
    {
        writer.Write(JsonSerializer.Serialize(report));
    }
}
