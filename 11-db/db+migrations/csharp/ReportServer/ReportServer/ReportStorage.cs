namespace ReportServer;

public class ReportStorage
{
    private readonly Dictionary<Guid, Report> _reports = new();
    private Guid? _lastReportId;

    public Guid StoreReport(Report report)
    {
        var id = Guid.NewGuid();
        _reports[id] = report;
        _lastReportId = id;
        return id;
    }

    public Report? GetReport(Guid id)
    {
        return _reports.TryGetValue(id, out var report) ? report : null;
    }

    public (Guid? Id, Report? Report) GetLastReport()
    {
        if (_lastReportId == null)
            return (null, null);

        var report = GetReport(_lastReportId.Value);
        return (_lastReportId, report);
    }
} 