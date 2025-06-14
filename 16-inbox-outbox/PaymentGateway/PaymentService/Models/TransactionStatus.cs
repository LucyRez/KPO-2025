namespace PaymentService.Models;

internal sealed class TransactionStatus
{
    public Guid Id { get; }
    public Guid TransactionId { get; }
    public TransactionStatusType Status { get; }
    public DateTimeOffset CreatedAt { get; }

    public TransactionStatus(Guid id, Guid transactionId, TransactionStatusType status, DateTimeOffset createdAt)
    {
        Id = id;
        TransactionId = transactionId;
        Status = status;
        CreatedAt = createdAt;
    }
}