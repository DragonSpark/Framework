using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class RegisterInitialization : ICommand<Action>, ICommand<Task>, ICommand<IOperation>
{
    public static RegisterInitialization Default { get; } = new();

    RegisterInitialization() : this(Commands.Default, Tasks.Default, Operations.Default) {}

    readonly IResult<List<Action>?>    _commands;
    readonly IResult<List<Task>?>      _tasks;
    readonly IResult<List<IOperation>?> _operations;

    public RegisterInitialization(IResult<List<Action>?> commands, IResult<List<Task>?> tasks,
                                  IResult<List<IOperation>?> operations)
    {
        _commands   = commands;
        _tasks      = tasks;
        _operations = operations;
    }

    public void Execute(Action parameter)
    {
        _commands.Get().Verify().Add(parameter);
    }

    public void Execute(Task parameter)
    {
        _tasks.Get().Verify().Add(parameter);
    }

    public void Execute(IOperation parameter)
    {
        _operations.Get().Verify().Add(parameter);
    }
}