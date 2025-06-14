namespace PaymentService.Models;

internal sealed class Account
{
    public Guid Id { get; }
    public decimal Balance { get; }
    public decimal Hold { get; }

    public Account(Guid id, decimal balance, decimal hold)
    {
        Id = id;
        Balance = balance;
        Hold = hold;
    }
}