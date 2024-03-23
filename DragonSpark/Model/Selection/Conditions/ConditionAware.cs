using JetBrains.Annotations;

namespace DragonSpark.Model.Selection.Conditions;

[UsedImplicitly]
public class ConditionAware<T> : IConditionAware<T>
{
	public ConditionAware(ICondition<T> condition) => Condition = condition;

	public ICondition<T> Condition { get; }
}