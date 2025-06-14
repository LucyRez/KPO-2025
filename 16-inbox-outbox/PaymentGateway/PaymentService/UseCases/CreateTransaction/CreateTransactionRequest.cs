namespace PaymentService.UseCases.CreateTransaction;

public sealed record CreateTransactionRequest(Guid SubjectId, Guid PeerId, decimal Amount);