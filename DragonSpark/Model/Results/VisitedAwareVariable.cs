using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Results;

public sealed class VisitedAwareVariable<T> : IMutationAware<T>
{
	readonly IMutable<T?>   _mutable;
	readonly IMutable<bool> _switch;

	public VisitedAwareVariable() : this(new Variable<T>(), new Switch()) {}

	public VisitedAwareVariable(IMutable<T?> mutable, IMutable<bool> @switch)
		: this(mutable, @switch,
		       @switch as ICondition ?? A.Result(@switch).Then().Accept<None>().Then().Out()) {}

	public VisitedAwareVariable(IMutable<T?> mutable, IMutable<bool> @switch, ICondition condition)
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