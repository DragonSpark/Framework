using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Runtime.Execution;

namespace DragonSpark.Model.Results;

public class AssignedAwareVariable<T> : IMutationAware<T>
{
	readonly IMutable<T?> _mutable;
	readonly ICounter     _counter;

	public AssignedAwareVariable() : this(new Variable<T>()) {}

	public AssignedAwareVariable(IMutable<T?> mutable) : this(mutable, new Counter()) {}

	public AssignedAwareVariable(IMutable<T?> mutable, ICounter counter)
		: this(mutable, counter, new HasCounted(counter)) {}

	public AssignedAwareVariable(IMutable<T?> mutable, ICounter counter, ICondition condition)
	{
		_mutable  = mutable;
		_counter  = counter;
		Condition = condition;
	}

	public ICondition<None> Condition { get; }

	public T Get() => _mutable.Get().Verify();

	public void Execute(T parameter)
	{
		_counter.Execute();
		_mutable.Execute(parameter);
	}
}