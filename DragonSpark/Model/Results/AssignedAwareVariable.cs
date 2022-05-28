using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Results;

public sealed class AssignedAwareVariable<T> : IMutationAware<T?>
{
	readonly IMutable<T?> _mutable;

	public AssignedAwareVariable() : this(new Variable<T>()) {}

	public AssignedAwareVariable(IMutable<T?> mutable) : this(mutable, new IsAssigned<T>(mutable)) {}

	public AssignedAwareVariable(IMutable<T?> mutable, ICondition condition)
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