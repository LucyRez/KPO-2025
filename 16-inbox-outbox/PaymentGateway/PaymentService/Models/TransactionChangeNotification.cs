namespace PaymentService.Models;

internal sealed class TransactionChangeNotification
{
    public Guid Id { get; }
    public string Payload { get; }
    public bool IsSent { get; }
    public DateTimeOffset CreatedAt { get; }

    public TransactionChangeNotification(Guid id, string payload, bool isSent, DateTimeOffset createdAt)
    {
        Id = id;
        Payload = payload;
        IsSent = isSent;
        CreatedAt = createdAt;
    }
}