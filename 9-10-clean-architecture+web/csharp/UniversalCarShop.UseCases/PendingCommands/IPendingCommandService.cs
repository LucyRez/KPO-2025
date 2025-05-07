namespace UniversalCarShop.UseCases.PendingCommands;

public interface IPendingCommandService
{
    void AddCommand(IAccountingSessionCommand command);
    void UndoLastCommand();
    void RedoUndoneCommand();
    void SaveChanges();
}

