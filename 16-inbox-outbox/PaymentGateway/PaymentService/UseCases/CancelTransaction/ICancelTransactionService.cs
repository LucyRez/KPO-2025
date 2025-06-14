namespace PaymentService.UseCases.CancelTransaction;

public interface ICancelTransactionService
{
    Task<CancelTransactionResponse> CancelTransactionAsync(CancelTransactionRequest request, CancellationToken cancellationToken);
}