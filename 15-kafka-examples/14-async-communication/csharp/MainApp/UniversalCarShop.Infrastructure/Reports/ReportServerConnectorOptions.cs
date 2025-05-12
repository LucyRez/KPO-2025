namespace UniversalCarShop.Infrastructure.Reports;

internal sealed record ReportServerConnectorOptions(
    string Topic,
    string BootstrapServers
);
