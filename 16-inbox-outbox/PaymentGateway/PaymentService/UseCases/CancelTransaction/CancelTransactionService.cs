using Microsoft.EntityFrameworkCore;
using PaymentService.Common.SendNotification;
using PaymentService.Database;
using PaymentService.Models;

using TransactionModel = PaymentService.Models.Transaction;
using TransactionStatusModel = PaymentService.Models.TransactionStatus;

namespace PaymentService.UseCases.CancelTransaction;

internal sealed class CancelTransactionService : ICancelTransactionService
{
    private readonly PaymentContext _context;
    private readonly ISendNotificationService _sendNotificationService;

    public CancelTransactionService(PaymentContext context, ISendNotificationService sendNotificationService)
    {
        _context = context;
        _sendNotificationService = sendNotificationService;
    }

    public async Task<CancelTransactionResponse> CancelTransactionAsync(CancelTransactionRequest request, CancellationToken cancellationToken)
    {
        do
        {
            cancellationToken.ThrowIfCancellationRequested();

            await using var tx = await _context.Database.BeginTransactionAsync(cancellationToken);

            var transaction = await _context.Transactions.FindAsync(request.TransactionId, cancellationToken);

            if (transaction is null)
            {
                throw new InvalidOperationException("Transaction not found");
            }

            if (!await TryCancelHoldAsync(transaction, cancellationToken))
            {
                await tx.RollbackAsync(cancellationToken);
                continue;
            }

            var transactionStatus = new TransactionStatusModel(
                Guid.NewGuid(),
                request.TransactionId,
                TransactionStatusType.Cancel,
                DateTimeOffset.UtcNow
            );

            await _context.TransactionStatuses.AddAsync(transactionStatus, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _sendNotificationService.SendTransactionChangeNotificationAsync(transaction, transactionStatus.Status, cancellationToken);

            await tx.CommitAsync(cancellationToken);

            return new CancelTransactionResponse();
        } while (true);
    }

    private async Task<bool> TryCancelHoldAsync(TransactionModel transaction, CancellationToken cancellationToken)
    {
        var lastStatus = await _context.TransactionStatuses
            .Where(s => s.TransactionId == transaction.Id)
            .OrderByDescending(s => s.CreatedAt)
            .FirstAsync(cancellationToken);

        if (lastStatus.Status != TransactionStatusType.Hold)
        {
            throw new InvalidOperationException("Transaction cannot be cancelled");
        }

        var account = await _context.Accounts.FindAsync(transaction.SubjectId, cancellationToken);

        if (account is null)
        {
            throw new InvalidOperationException("Account not found");
        }

        var oldBalance = account.Balance;
        var oldHold = account.Hold;
        var newBalance = oldBalance + transaction.Amount;
        var newHold = oldHold - transaction.Amount;

        var updatedRows = await _context.Accounts
            .Where(a => a.Id == account.Id)
            .Where(a => a.Balance == oldBalance)
            .Where(a => a.Hold == oldHold)
            .ExecuteUpdateAsync(
                a => a.SetProperty(a => a.Balance, a => newBalance)
                    .SetProperty(a => a.Hold, a => newHold),
                cancellationToken
            );

        if (updatedRows != 1)
        {
            return false;
        }

        return true;
    }
}