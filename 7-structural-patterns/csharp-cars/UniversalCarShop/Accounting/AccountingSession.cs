using System;
using UniversalCarShop.Reports;

namespace UniversalCarShop.Accounting;

/// <summary>
/// Сеанс работы с системой учета.
/// </summary>
public sealed class AccountingSession
{
    // Список непримененных команд.
    private readonly LinkedList<IAccountingSessionCommand> _unappliedCommands = new();
    // Список отмененных команд.
    private readonly Stack<IAccountingSessionCommand> _undoneCommands = new();

    private readonly ReportBuilder _reportBuilder;

    /// <summary>
    /// Конструктор класса.
    /// </summary>
    /// <param name="reportBuilder">Построитель отчета.</param>
    public AccountingSession(ReportBuilder reportBuilder)
    {
        _reportBuilder = reportBuilder;
    }

    /// <summary>
    /// Добавление команды в сеанс.
    /// </summary>
    /// <param name="command">Команда.</param>
    public void AddCommand(IAccountingSessionCommand command)
    {
        // Добавляем команду в список непримененных команд.
        _unappliedCommands.AddLast(command);
        // Очищаем список отмененных команд, так как они становятся недействительными.
        _undoneCommands.Clear();
    }

    /// <summary>
    /// Список непримененных команд.
    /// </summary>
    public IReadOnlyCollection<IAccountingSessionCommand> UnappliedCommands => _unappliedCommands;

    /// <summary>
    /// Отмена последней команды.
    /// </summary>
    public void UndoLastCommand()
    {
        // Если список непримененных команд не пуст.
        if (_unappliedCommands.Last is not null)
        {
            // Получаем последнюю команду из списка непримененных команд.
            var command = _unappliedCommands.Last.Value;
            // Удаляем последнюю команду из списка непримененных команд.
            _unappliedCommands.RemoveLast();
            // Добавляем команду в список отмененных команд.
            _undoneCommands.Push(command);
        }
    }

    /// <summary>
    /// Повтор отмененной команды.
    /// </summary>
    public void RedoUndoneCommand()
    {
        // Если есть отмененные команды.
        if (_undoneCommands.Count > 0)
        {
            // Получаем последнюю отмененную команду.
            var command = _undoneCommands.Pop();
            // Добавляем команду в список непримененных команд.
            _unappliedCommands.AddLast(command);
        }
    }

    /// <summary>
    /// Сохранение внесенных изменений.
    /// </summary>
    public void SaveChanges()
    {
        // Проходимся по всем непримененным командам.
        foreach (var command in _unappliedCommands)
        {
            // Применяем команду.
            command.Apply();

            // Добавляем команду в отчет.
            var operation = command.ToString();

            if (operation is not null)
            {
                _reportBuilder.AddOperation(operation);
            }
        }
    }
}
