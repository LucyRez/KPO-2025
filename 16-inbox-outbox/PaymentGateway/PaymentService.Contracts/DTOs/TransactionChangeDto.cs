namespace PaymentService.Contracts.DTOs;

/// <summary>
/// Информация о транзакции
/// </summary>
/// <param name="TransactionId">Идентификатор транзакции</param>
/// <param name="SubjectId">Идентификатор счета, над которым происходит транзакция</param>
/// <param name="PeerId">Идентификатор счета, с которым происходит транзакция</param>
/// <param name="Amount">Сумма транзакции</param>
/// <param name="Status">Статус транзакции</param>
public sealed record TransactionChangeDto(
    Guid TransactionId,
    Guid SubjectId,
    Guid PeerId,
    decimal Amount,
    TransactionStatusType Status,
    DateTimeOffset UpdatedAt);