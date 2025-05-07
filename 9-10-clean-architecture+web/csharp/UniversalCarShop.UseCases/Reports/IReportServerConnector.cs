using UniversalCarShop.Entities.Common;

namespace UniversalCarShop.UseCases.Reports;

public interface IReportServerConnector
{
    void StoreReport(Report report);
}