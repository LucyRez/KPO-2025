using System;
using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.UseCases.Reports;

public abstract class ReportExporter
{
    /// <summary>
    /// Метод для экспорта отчета в определенный формат.
    /// </summary>
    /// <param name="report">Отчет для экспорта.</param>
    /// <param name="writer">Ссылка на поток вывода.</param>
    public abstract void Export(Report report, TextWriter writer);
}
