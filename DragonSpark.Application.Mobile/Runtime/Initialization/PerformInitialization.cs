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

    PerformInitialization() : this(Commands.Default, Tasks.Default, Operations.Default) {}

    readonly IMutable<List<Action>?>     _commands;
    readonly IMutable<List<Task>?>       _tasks;
    readonly IMutable<List<IOperation>?> _operations;

    public PerformInitialization(IMutable<List<Action>?> commands, IMutable<List<Task>?> tasks,
                                 IMutable<List<IOperation>?> operations)
    {
        _commands   = commands;
        _tasks      = tasks;
        _operations = operations;
    }

    public async ValueTask Get()
    {
        if (_tasks.TryPop(out var tasks) && tasks is not null)
        {
            await Task.WhenAll(tasks).Off();
        }

        if (_operations.TryPop(out var operations) && operations is not null)
        {
            foreach (var operation in operations)
            {
                await operation.Off();
            }
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