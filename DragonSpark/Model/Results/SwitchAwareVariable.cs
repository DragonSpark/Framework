using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Results;

public sealed class SwitchAwareVariable<T> : IMutationAware<T>
{
	readonly IMutable<T?> _mutable;
	readonly ISwitch      _switch;

	public SwitchAwareVariable() : this(new Variable<T>(), false) {}

	public SwitchAwareVariable(IMutable<T?> mutable, Switch @switch) : this(mutable, @switch, @switch) {}

	public SwitchAwareVariable(IMutable<T?> mutable, ISwitch @switch, ICondition condition)
	{
		_mutable  = mutable;
		_switch   = @switch;
		Condition = condition;
	}

	public ICondition<None> Condition { get; }

	public T Get() => _mutable.Get().Verify();

	public void Execute(T parameter)
	{
		_switch.Up();
		_mutable.Execute(parameter);
	}
}