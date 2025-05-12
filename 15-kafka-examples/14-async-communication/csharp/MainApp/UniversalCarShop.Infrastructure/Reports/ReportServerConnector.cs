using System.Text.Json;
using UniversalCarShop.UseCases.Reports;
using Confluent.Kafka;

namespace UniversalCarShop.Infrastructure.Reports;

internal sealed class ReportServerConnector : IReportServerConnector, IDisposable
{
    private readonly IProducer<string, string> _producer;
    private readonly string _topic;

    public ReportServerConnector(
        IProducer<string, string> producer,
        string topic)
    {
        _producer = producer;
        _topic = topic;
    }

    public void SendEvent(ReportedEventDto reportedEventDto)
    {
        var message = new Message<string, string> { Value = JsonSerializer.Serialize(reportedEventDto) };

        _producer.Produce(_topic, message);
    }

    public void Dispose()
    {
        _producer.Dispose();
    }
}

