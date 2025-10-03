using System;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class PerformInitialization : IOperation
{
    public static PerformInitialization Default { get; } = new();

    PerformInitialization() : this(Commands.Default, Operations.Default) {}

    readonly ICommands<Action>       _commands;
    readonly IOperations<IOperation> _operations;

    public PerformInitialization(ICommands<Action> commands, IOperations<IOperation> operations)
    {
        _commands   = commands;
        _operations = operations;
    }

    public async ValueTask Get()
    {
        await _operations.Off();
        _commands.Execute();
    }
}