namespace PaymentService.Models;

/// <summary>
/// Тип статуса транзакции
/// </summary>
public enum TransactionStatusType
{
    /// <summary>
    /// Заморозка средств на исходном счету
    /// </summary>
    Hold,
    /// <summary>
    /// Перевод средств на целевой счет
    /// </summary>
    Charge,
    /// <summary>
    /// Отмена транзакции
    /// </summary>
    Cancel,
}