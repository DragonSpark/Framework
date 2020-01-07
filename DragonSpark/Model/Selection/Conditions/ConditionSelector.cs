using DragonSpark.Compose;

namespace DragonSpark.Model.Selection.Conditions
{
	public sealed class ConditionSelector : Select<ICondition, bool>
	{
		public static ConditionSelector Default { get; } = new ConditionSelector();

		ConditionSelector() : base(x => x.Get()) {}
	}
}