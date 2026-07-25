using DragonSpark.Compose;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class Initialization : IStopAware
{
    public static Initialization Default { get; } = new();

    Initialization() : this(Commands.Default, Operations.Default) {}

    readonly ICommands<Action>       _commands;
    readonly IOperations<IStopAware> _operations;

    public Initialization(ICommands<Action> commands, IOperations<IStopAware> operations)
    {
        _commands   = commands;
        _operations = operations;
    }

    public async ValueTask Get(CancellationToken parameter)
    {
        await _operations.Off(parameter);
        _commands.Execute();
    }
}