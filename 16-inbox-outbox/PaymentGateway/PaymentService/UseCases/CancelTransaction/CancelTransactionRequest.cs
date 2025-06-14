namespace PaymentService.UseCases.CancelTransaction;

public sealed record CancelTransactionRequest(Guid TransactionId);