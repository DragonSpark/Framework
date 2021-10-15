using DragonSpark.Runtime.Activation;

namespace DragonSpark.Model.Selection.Conditions;

public class FixedResultCondition<T> : ICondition<T>, IActivateUsing<bool>
{
	readonly bool _result;

	public FixedResultCondition(bool result) => _result = result;

	public bool Get(T _) => _result;
}