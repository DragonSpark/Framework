using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Application.Mobile.Runtime.Initialization;

public class StateSwitch : ISwitch, ICondition
{
    readonly IMutable<bool> _previous;

    protected StateSwitch() : this(new Variable<bool>().Protected()) {}

    protected StateSwitch(IMutable<bool> previous) => _previous = previous;

    public bool Get() => _previous.Get();

    public void Execute(bool parameter)
    {
        _previous.Execute(parameter);
    }

    public bool Get(None parameter) => Get();
}