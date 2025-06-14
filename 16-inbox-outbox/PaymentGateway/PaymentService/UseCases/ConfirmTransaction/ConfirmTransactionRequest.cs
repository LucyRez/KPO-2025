namespace PaymentService.UseCases.ConfirmTransaction;

public sealed record ConfirmTransactionRequest(Guid TransactionId);