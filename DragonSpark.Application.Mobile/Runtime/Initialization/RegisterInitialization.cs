using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class RegisterInitialization : ICommand<Action>, ICommand<Task>
{
    public static RegisterInitialization Default { get; } = new();

    RegisterInitialization() : this(Commands.Default, Operations.Default) {}

    readonly IResult<List<Action>?> _commands;
    readonly IResult<List<Task>?>   _operations;

    public RegisterInitialization(IResult<List<Action>?> commands, IResult<List<Task>?> operations)
    {
        _commands   = commands;
        _operations = operations;
    }

    public void Execute(Action parameter)
    {
        _commands.Get().Verify().Add(parameter);
    }

    public void Execute(Task parameter)
    {
        _operations.Get().Verify().Add(parameter);
    }
}