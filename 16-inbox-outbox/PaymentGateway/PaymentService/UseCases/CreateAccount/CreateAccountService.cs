using PaymentService.Database;

namespace PaymentService.UseCases.CreateAccount;

internal sealed class CreateAccountService : ICreateAccountService
{
    private readonly PaymentContext _context;

    public CreateAccountService(PaymentContext context)
    {
        _context = context;
    }

    public async Task<CreateAccountResponse> CreateAccountAsync(CreateAccountRequest request, CancellationToken cancellationToken)
    {
        var account = new Models.Account(Guid.NewGuid(), hold: 0, balance: request.Balance);

        await _context.Accounts.AddAsync(account, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateAccountResponse(account.Id);
    }
}