using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Results;

public class MutationAware<T> : IMutationAware<T?>
{
	readonly IMutable<T?> _mutable;

	public MutationAware(ICondition condition) : this(new Variable<T>(), condition) {}

	public MutationAware(IMutable<T?> mutable, ICondition condition)
	{
		_mutable  = mutable;
		Condition = condition;
	}

	public ICondition<None> Condition { get; }

	public T? Get() => _mutable.Get();

	public void Execute(T? parameter)
	{
		_mutable.Execute(parameter);
	}
}