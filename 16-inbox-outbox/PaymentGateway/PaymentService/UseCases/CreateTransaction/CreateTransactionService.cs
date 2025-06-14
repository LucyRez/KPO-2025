using System.Transactions;
using Microsoft.EntityFrameworkCore;
using PaymentService.Common.SendNotification;
using PaymentService.Database;
using PaymentService.Models;

using TransactionModel = PaymentService.Models.Transaction;
using TransactionStatusModel = PaymentService.Models.TransactionStatus;

namespace PaymentService.UseCases.CreateTransaction;

internal sealed class CreateTransactionService : ICreateTransactionService
{
    private readonly PaymentContext _context;
    private readonly ISendNotificationService _sendNotificationService;

    public CreateTransactionService(PaymentContext context, ISendNotificationService sendNotificationService)
    {
        _context = context;
        _sendNotificationService = sendNotificationService;
    }

    public async Task<CreateTransactionResponse> CreateTransactionAsync(CreateTransactionRequest request, CancellationToken cancellationToken)
    {
        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);

            if (!await TryHoldAsync(request.SubjectId, request.Amount, cancellationToken))
            {
                await tx.RollbackAsync(cancellationToken);
                continue;
            }
            
            var (transaction, transactionStatus) = await CreateTransactionAsync(request.SubjectId, request.PeerId, request.Amount, cancellationToken);

            await _sendNotificationService.SendTransactionChangeNotificationAsync(transaction, transactionStatus.Status, cancellationToken);

            await tx.CommitAsync(cancellationToken);

            return new CreateTransactionResponse(transaction.Id);
        } while (true);
    }

    private async Task<bool> TryHoldAsync(Guid accountId, decimal amount, CancellationToken cancellationToken)
    {
        var account = await _context.Accounts.FindAsync(accountId, cancellationToken);

        if (account is null)
        {
            throw new InvalidOperationException("Account not found");
        }

        if (account.Balance < amount)
        {
            throw new InvalidOperationException("Insufficient balance");
        }

        var oldBalance = account.Balance;
        var newBalance = oldBalance - amount;

        if (newBalance < 0)
        {
            throw new InvalidOperationException("Insufficient balance");
        }

        var oldHold = account.Hold;
        var newHold = oldHold + amount;
        
        var updatedRows = await _context.Accounts
            .Where(a => a.Id == account.Id)
            .Where(a => a.Balance == oldBalance)
            .Where(a => a.Hold == oldHold)
            .ExecuteUpdateAsync(
                a => a.SetProperty(a => a.Balance, a => newBalance)
                    .SetProperty(a => a.Hold, a => newHold),
                cancellationToken
            );

        if (updatedRows == 1)
        {
            return true;
        }

        return false;
    }

    private async Task<(TransactionModel, TransactionStatusModel)> CreateTransactionAsync(Guid subjectId, Guid peerId, decimal amount, CancellationToken cancellationToken)
    {
        var transaction = new TransactionModel(
            Guid.NewGuid(),
            subjectId,
            peerId,
            amount,
            DateTimeOffset.UtcNow
        );

        var transactionStatus = new TransactionStatusModel(
            Guid.NewGuid(),
            transaction.Id,
            TransactionStatusType.Hold,
            DateTimeOffset.UtcNow
        );

        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.TransactionStatuses.AddAsync(transactionStatus, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return (transaction, transactionStatus);
    }
}