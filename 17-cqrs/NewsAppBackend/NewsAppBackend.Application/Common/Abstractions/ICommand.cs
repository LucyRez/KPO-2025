namespace NewsAppBackend.Application.Common.Abstractions;

public interface ICommand
{
}

public interface ICommand<out TResult> : ICommand
{
}