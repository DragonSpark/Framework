using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class RegisterCommand : ICommand<Action>
{
    public static RegisterCommand Default { get; } = new();

    RegisterCommand() : this(Commands.Default, Started.Default, RunCommand.Default) {}

    readonly IResult<List<Action>> _commands;
    readonly ICondition            _started;
    readonly ICommand<Action>      _run;

    public RegisterCommand(IResult<List<Action>> commands, ICondition started, ICommand<Action> run)
    {
        _commands = commands;
        _started  = started;
        _run      = run;
    }

    public void Execute(Action parameter)
    {
        if (_started.Get())
        {
            _run.Execute(parameter);
        }
        else
        {
            _commands.Get().Add(parameter);
        }
    }
}