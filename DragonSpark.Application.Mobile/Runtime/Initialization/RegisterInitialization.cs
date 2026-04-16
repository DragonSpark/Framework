using System;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations.Stop;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public sealed class RegisterInitialization : ICommand<Action>, ICommand<IStopAware>
{
    public static RegisterInitialization Default { get; } = new();

    RegisterInitialization() : this(RegisterOperation.Default, RegisterCommand.Default) {}

    readonly ICommand<IStopAware> _operation;
    readonly ICommand<Action>     _command;

    public RegisterInitialization(ICommand<IStopAware> operation, ICommand<Action> command)
    {
        _operation = operation;
        _command   = command;
    }

    public void Execute(Action parameter)
    {
        _command.Execute(parameter);
    }

    public void Execute(IStopAware parameter)
    {
        _operation.Execute(parameter);
    }
}