namespace DragonSpark.Model.Selection.Conditions
{
	public class ConditionAware<T> : IConditionAware<T>
	{
		public ConditionAware(ICondition<T> condition) => Condition = condition;

		public ICondition<T> Condition { get; }
	}
}