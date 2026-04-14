using System;
using System.Collections.Generic;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class RegisterInitialization : ICommand<Action>, ICommand<IOperation>
{
    public static RegisterInitialization Default { get; } = new();

    RegisterInitialization() : this(Commands.Default, Operations.Default) {}

    readonly IResult<List<Action>?>     _commands;
    readonly IResult<List<IOperation>?> _operations;

    public RegisterInitialization(IResult<List<Action>?> commands, IResult<List<IOperation>?> operations)
    {
        _commands   = commands;
        _operations = operations;
    }

    public void Execute(Action parameter)
    {
        var actions = _commands.Get();
        if (actions is not null)
        {
            actions.Add(parameter);
        }
        else
        {
            parameter();
        }
    }

    public void Execute(IOperation parameter)
    {
        var operations = _operations.Get();
        if (operations is not null)
        {
            operations.Add(parameter);
        }
        else
        {
            _ = parameter.Get();
        }
    }
}