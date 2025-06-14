namespace PaymentService.UseCases.CreateTransaction;

public interface ICreateTransactionService
{
    Task<CreateTransactionResponse> CreateTransactionAsync(CreateTransactionRequest request, CancellationToken cancellationToken);
}