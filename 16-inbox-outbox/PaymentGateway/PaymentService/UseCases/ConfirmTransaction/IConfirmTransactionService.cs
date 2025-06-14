namespace PaymentService.UseCases.ConfirmTransaction;

public interface IConfirmTransactionService
{
    Task<ConfirmTransactionResponse> ConfirmTransactionAsync(ConfirmTransactionRequest request, CancellationToken cancellationToken);
}