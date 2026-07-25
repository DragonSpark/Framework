using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

sealed class RegisterOperation : ICommand<IStopAware>
{
    public static RegisterOperation Default { get; } = new();

    RegisterOperation() : this(Operations.Default, Started.Default, RunOperation.Default) {}

    readonly IResult<List<IStopAware>> _list;
    readonly ICondition                _started;
    readonly IOperation<IStopAware>    _run;

    public RegisterOperation(IResult<List<IStopAware>> list, ICondition started, IOperation<IStopAware> run)
    {
        _list    = list;
        _started = started;
        _run     = run;
    }

    public void Execute(IStopAware parameter)
    {
        if (_started.Get())
        {
            _ = _run.Get(parameter);
        }
        else
        {
            _list.Get().Add(parameter);
        }
    }
}