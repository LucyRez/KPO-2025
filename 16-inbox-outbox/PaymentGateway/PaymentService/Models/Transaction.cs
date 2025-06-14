namespace PaymentService.Models;

internal sealed class Transaction
{
    public Guid Id { get; }
    public Guid SubjectId { get; }
    public Guid PeerId { get; }
    public decimal Amount { get; }
    public DateTimeOffset CreatedAt { get; }

    public Transaction(Guid id, Guid subjectId, Guid peerId, decimal amount, DateTimeOffset createdAt)
    {
        Id = id;
        SubjectId = subjectId;
        PeerId = peerId;
        Amount = amount;
        CreatedAt = createdAt;
    }
}