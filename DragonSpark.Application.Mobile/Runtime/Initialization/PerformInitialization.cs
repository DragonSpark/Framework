using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class PerformInitialization : IOperation
{
    public static PerformInitialization Default { get; } = new();

    PerformInitialization() : this(Commands.Default, Operations.Default) {}

    readonly IMutable<List<Action>?> _commands;
    readonly IMutable<List<Task>?>   _operations;

    public PerformInitialization(IMutable<List<Action>?> commands, IMutable<List<Task>?> operations)
    {
        _commands   = commands;
        _operations = operations;
    }

    public async ValueTask Get()
    {
        if (_operations.TryPop(out var operations) && operations is not null)
        {
            await Task.WhenAll(operations).Off();
        }

        if (_commands.TryPop(out var commands) && commands is not null)
        {
            foreach (var command in commands)
            {
                command();
            }
        }
    }
}