using Microsoft.EntityFrameworkCore;
using PaymentService.Common.SendNotification;
using PaymentService.Database;
using PaymentService.Models;
using TransactionModel = PaymentService.Models.Transaction;
using TransactionStatusModel = PaymentService.Models.TransactionStatus;

namespace PaymentService.UseCases.ConfirmTransaction;

internal sealed class ConfirmTransactionService : IConfirmTransactionService
{
    private readonly PaymentContext _context;
    private readonly ISendNotificationService _sendNotificationService;

    public ConfirmTransactionService(PaymentContext context, ISendNotificationService sendNotificationService)
    {
        _context = context;
        _sendNotificationService = sendNotificationService;
    }

    public async Task<ConfirmTransactionResponse> ConfirmTransactionAsync(ConfirmTransactionRequest request, CancellationToken cancellationToken)
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

            if (!await TryConfirmTransactionAsync(transaction, cancellationToken))
            {
                await tx.RollbackAsync(cancellationToken);
                continue;
            }

            var transactionStatus = new TransactionStatusModel(
                Guid.NewGuid(),
                request.TransactionId,
                TransactionStatusType.Charge,
                DateTimeOffset.UtcNow
            );

            await _context.TransactionStatuses.AddAsync(transactionStatus, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await _sendNotificationService.SendTransactionChangeNotificationAsync(transaction, transactionStatus.Status, cancellationToken);

            await tx.CommitAsync(cancellationToken);

            return new ConfirmTransactionResponse();
        } while (true);
    }

    private async Task<bool> TryConfirmTransactionAsync(TransactionModel transaction, CancellationToken cancellationToken)
    {
        var lastStatus = await _context.TransactionStatuses
            .Where(s => s.TransactionId == transaction.Id)
            .OrderByDescending(s => s.CreatedAt)
            .FirstAsync(cancellationToken);

        if (lastStatus.Status != TransactionStatusType.Hold)
        {
            throw new InvalidOperationException("Transaction cannot be confirmed");
        }

        if (!await TryUpdatePeerAccountAsync(transaction.PeerId, transaction.Amount, cancellationToken))
        {
            return false;
        }

        if (!await TryUpdateSubjectAccountAsync(transaction.SubjectId, transaction.Amount, cancellationToken))
        {
            return false;
        }

        return true;
    }

    private async Task<bool> TryUpdatePeerAccountAsync(Guid peerId, decimal amount, CancellationToken cancellationToken)
    {
        var peerAccount = await _context.Accounts.FindAsync(peerId, cancellationToken);

        if (peerAccount is null)
        {
            throw new InvalidOperationException("Peer account not found");
        }

        var oldBalance = peerAccount.Balance;
        var newBalance = oldBalance + amount;

        var updatedRows = await _context.Accounts
            .Where(a => a.Id == peerAccount.Id)
            .Where(a => a.Balance == oldBalance)
            .ExecuteUpdateAsync(
                a => a.SetProperty(a => a.Balance, a => newBalance),
                cancellationToken
            );

        if (updatedRows != 1)
        {
            return false;
        }

        return true;
    }

    private async Task<bool> TryUpdateSubjectAccountAsync(Guid subjectId, decimal amount, CancellationToken cancellationToken)
    {
        var subjectAccount = await _context.Accounts.FindAsync(subjectId, cancellationToken);

        if (subjectAccount is null)
        {
            throw new InvalidOperationException("Subject account not found");
        }

        var oldHold = subjectAccount.Hold;
        var newHold = oldHold - amount;

        var updatedRows = await _context.Accounts
            .Where(a => a.Id == subjectAccount.Id)
            .Where(a => a.Hold == oldHold)
            .ExecuteUpdateAsync(
                a => a.SetProperty(a => a.Hold, a => newHold),
                cancellationToken
            );

        if (updatedRows != 1)
        {
            return false;
        }

        return true;
    }
}