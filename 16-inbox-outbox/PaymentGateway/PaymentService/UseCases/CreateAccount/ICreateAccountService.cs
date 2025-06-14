namespace PaymentService.UseCases.CreateAccount;

internal interface ICreateAccountService
{
    Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest request, CancellationToken cancellationToken);
}