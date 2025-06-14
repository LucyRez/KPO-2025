using PaymentService.Models;

namespace PaymentService.Common.SendNotification;

internal interface ISendNotificationService
{
    Task SendTransactionChangeNotificationAsync(Transaction transaction, TransactionStatusType status, CancellationToken cancellationToken);
}