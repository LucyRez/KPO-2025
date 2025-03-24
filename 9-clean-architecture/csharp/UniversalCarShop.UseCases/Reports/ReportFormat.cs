using System;

namespace UniversalCarShop.UseCases.Reports;

/// <summary>
/// Перечисление для обозначения формата экспорта.
/// </summary>
public enum ReportFormat
{
    /// <summary>
    /// Формат JSON.
    /// </summary>
    Json,
    /// <summary>
    /// Формат Markdown.
    /// </summary>
    Markdown,

    /// <summary>
    /// Отправка отчета на сервер.
    /// </summary>
    Server
}
