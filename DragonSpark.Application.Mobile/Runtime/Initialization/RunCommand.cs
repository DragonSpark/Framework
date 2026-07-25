using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class RunCommand : ICommand<Action>
{
    public static RunCommand Default { get; } = new();

    RunCommand() : this(HandleInitializationException.Default) {}

    readonly ICommand<Exception> _exception;

    public RunCommand(ICommand<Exception> exception)
    {
        _exception = exception;
    }

    public void Execute(Action parameter)
    {
        try
        {
            parameter();
        }
        catch (Exception e)
        {
            _exception.Execute(e);
            throw;
        }
    }
}