namespace UniversalCarShop.UseCases.Reports;

public interface IReportServerConnector
{
    void SendEvent(ReportedEventDto reportedEventDto);
}