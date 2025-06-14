using System.Text.Json;
using PaymentService.Contracts.DTOs;
using PaymentService.Database;
using PaymentService.Models;

using ContractsStatus = PaymentService.Contracts.DTOs.TransactionStatusType;
using ModelsStatus = PaymentService.Models.TransactionStatusType;

namespace PaymentService.Common.SendNotification;

internal sealed class SendNotificationService : ISendNotificationService
{
    private readonly PaymentContext _context;

    public SendNotificationService(PaymentContext context)
    {
        _context = context;
    }

    public async Task SendTransactionChangeNotificationAsync(Transaction transaction, ModelsStatus status, CancellationToken cancellationToken)
    {
        var payload = new TransactionChangeDto(
            transaction.Id,
            transaction.SubjectId,
            transaction.PeerId,
            transaction.Amount,
            ToContractStatus(status),
            transaction.CreatedAt
        );

        var payloadJson = JsonSerializer.Serialize(payload);

        var notification = new TransactionChangeNotification(
            Guid.NewGuid(),
            payloadJson,
            isSent: false,
            DateTimeOffset.UtcNow
        );

        await _context.TransactionChangeNotifications.AddAsync(notification, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    private static ContractsStatus ToContractStatus(ModelsStatus status)
    {
        return status switch
        {
            ModelsStatus.Hold => ContractsStatus.Hold,
            ModelsStatus.Charge => ContractsStatus.Charge,
            ModelsStatus.Cancel => ContractsStatus.Cancel,
            _ => throw new ArgumentException($"Invalid transaction status: {status}", nameof(status))
        };
    }
}
