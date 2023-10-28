using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Results;

public class MutationAware<T> : Mutable<T?>, IMutationAware<T?>
{
	public MutationAware(ICondition condition) : this(new Variable<T>(), condition) {}

	protected MutationAware(IMutable<T?> mutable, ICondition condition) : base(mutable) => Condition = condition;

	public ICondition<None> Condition { get; }
}